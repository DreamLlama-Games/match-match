using System.Collections;
using System.Collections.Generic;
using GameLogicScripts;
using MyUtilityScripts;
using UnityEngine;

namespace VisualAnimationScripts
{
    public class CardAnimationHandler
    {
        private readonly MonoBehaviour _animationOwner;
        private readonly Dictionary<RectTransform, Coroutine> _cardAnimationHandles = new();

        public CardAnimationHandler(MonoBehaviour animationOwner)
        {
            _animationOwner = animationOwner;
        }

        public void OnCardSelected(Card card,float flipDuration, float initialYRotation, float finalYRotation)
        {
            var handle = _animationOwner?.StartCoroutine(MyUtils.RunSequential(
                FlipCard(card, flipDuration, initialYRotation, finalYRotation),
                Wait(0.5f),
                FlipCard(card, flipDuration, finalYRotation, initialYRotation)
            ));
            
            if(!_cardAnimationHandles.TryGetValue(card.CardTransform, out _)) _cardAnimationHandles.Add(card.CardTransform, handle);
            _cardAnimationHandles[card.CardTransform] = handle;
        }

        public void OnTwinFound(Card twin1, Card twin2, float finalYRotation, float flashDuration, float flashScaleFactor, float shrinkScaleFactor, float moveDuration, RectTransform target)
        {
            twin1.ResetTransform();
            twin2.ResetTransform();
            
            //stop previous running coroutines
            if(_cardAnimationHandles.TryGetValue(twin1.CardTransform, out var handle1)) _animationOwner?.StopCoroutine(handle1);
            if(_cardAnimationHandles.TryGetValue(twin2.CardTransform, out var handle2)) _animationOwner?.StopCoroutine(handle2);
            
            twin1.CardTransform.localEulerAngles = new Vector3(0, finalYRotation, 0); // open state
            twin2.CardTransform.localEulerAngles = new Vector3(0, finalYRotation, 0); // open state
            
            _animationOwner?.StartCoroutine(MyUtils.RunSequential(
                MyUtils.RunParallel(_animationOwner,
                    FlashCard(twin1, flashDuration, flashScaleFactor), FlashCard(twin2, flashDuration, flashScaleFactor)),
                Wait(0.5f),
                MyUtils.RunParallel(
                    _animationOwner,
                    MoveCard(twin1,moveDuration,target), MoveCard(twin2, moveDuration, target),
                    FlashCard(twin1, moveDuration, shrinkScaleFactor), FlashCard(twin2, moveDuration, shrinkScaleFactor)
                    )
                ));
            
            //clear entries
            _cardAnimationHandles?.Remove(twin1.CardTransform);
            _cardAnimationHandles?.Remove(twin2.CardTransform);
        }
        
        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSeconds(duration);
        }
        
        private IEnumerator FlipCard(Card card, float duration,float initialYRotation, float finalYRotation)
        {
            var rect = card.CardTransform;
            var initialRotation = rect.localEulerAngles;
            rect.localEulerAngles = new Vector3(initialRotation.x, initialYRotation, initialRotation.z);
            
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var activeYRotation = Mathf.Lerp(initialYRotation, finalYRotation, elapsed / duration);
                rect.localEulerAngles = new Vector3(initialRotation.x, activeYRotation, initialRotation.z);
                yield return null;
            }
            
            rect.localEulerAngles = new Vector3(initialRotation.x, finalYRotation, initialRotation.z);
        }

        private IEnumerator FlashCard(Card card, float duration, float finalScaleFactor)
        {
            var rect = card.CardTransform;
            var initialScale = rect.localScale;
            var scaleOffsetFactor = finalScaleFactor - 1f; 
            
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var activeScaleFactor = scaleOffsetFactor * Mathf.InverseLerp(0f, duration, elapsed);
                rect.localScale = initialScale * (1 + activeScaleFactor);
                yield return null;
            }

            rect.localScale = initialScale * finalScaleFactor;
        }

        private IEnumerator MoveCard(Card card, float duration, RectTransform target)
        {
            var rect = card.CardTransform;
            var targetRelativeLocalPos = rect.parent.InverseTransformPoint(target.position);
            var startPos = rect.localPosition;
            
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                rect.localPosition = Vector3.Lerp(startPos, targetRelativeLocalPos, elapsed / duration);
                yield return null;
            }

            target.localPosition = targetRelativeLocalPos;
        }
    }
}