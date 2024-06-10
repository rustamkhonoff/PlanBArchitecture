using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace PlanB.Extensions
{
    public static class TMPExtensions
    {
        /// <summary>
        /// Finds Sprite by index and returns it middle position, if no results, returns Vector3.zero
        /// </summary>
        public static Vector3 GetSpritePosition(this TMP_Text tmpText, int spriteIndex)
        {
            List<TMP_CharacterInfo> charsList = tmpText.textInfo.characterInfo
                .Where(a => a.elementType == TMP_TextElementType.Sprite).ToList();
            if (charsList.Count == 0)
            {
                Debug.LogWarning("There is no any sprite in text", tmpText);
                return Vector3.zero;
            }

            foreach (TMP_CharacterInfo tmpCharacterInfo in charsList)
            {
                if (tmpCharacterInfo.elementType == TMP_TextElementType.Sprite &&
                    tmpCharacterInfo.spriteIndex == spriteIndex)
                {
                    Vector3 spritePosition = Vector3.Lerp(tmpCharacterInfo.bottomLeft, tmpCharacterInfo.topRight, 0.5f);
                    return tmpText.transform.TransformPoint(spritePosition);
                }
            }

            Debug.LogWarning($"There is no character with given sprite index, index = {spriteIndex}", tmpText);
            return Vector3.zero;
        }

        private static Dictionary<(TMP_SpriteAsset, int), Texture2D> CachedTMPSpriteTextures { get; set; } = new(16);

        /// <summary>
        /// Finds Sprite and returns Texture in TMP by index
        /// </summary>
        public static Texture2D GetSpriteTextureInIndex(this TMP_Text tmpText, int spriteIndex)
        {
            CachedTMPSpriteTextures = CachedTMPSpriteTextures
                .Where(texture => texture.Value != null)
                .ToDictionary(x => x.Key, x => x.Value);

            List<TMP_CharacterInfo> charsList = tmpText.textInfo.characterInfo
                .Where(a => a.elementType == TMP_TextElementType.Sprite).ToList();
            if (charsList.Count == 0)
            {
                Debug.LogWarning("There is no any sprite in text", tmpText);
                return null;
            }

            foreach (TMP_CharacterInfo tmpCharacterInfo in charsList)
            {
                if (tmpCharacterInfo.elementType == TMP_TextElementType.Sprite &&
                    tmpCharacterInfo.spriteIndex == spriteIndex)
                {
                    TMP_SpriteAsset asset = tmpCharacterInfo.spriteAsset;
                    if (CachedTMPSpriteTextures.TryGetValue((asset, spriteIndex), out Texture2D cachedTexture))
                        return cachedTexture;
                    Texture2D texture = asset.spriteSheet as Texture2D;
                    int width = (int)(Vector2.Distance(tmpCharacterInfo.bottomLeft, tmpCharacterInfo.bottomRight) /
                                      tmpCharacterInfo.scale);
                    int height = (int)(Vector2.Distance(tmpCharacterInfo.bottomLeft, tmpCharacterInfo.topLeft) /
                                       tmpCharacterInfo.scale);
                    int posX = (int)(tmpCharacterInfo.vertex_BL.uv.x * texture.height);
                    int posY = (int)(tmpCharacterInfo.vertex_BL.uv.y * texture.width);
                    Color[] colors = texture.GetPixels(posX, posY, width, height);
                    Texture2D newTexture = new(width, height);
                    newTexture.SetPixels(colors);
                    newTexture.Apply();
                    CachedTMPSpriteTextures.Add((asset, spriteIndex), newTexture);
                    return newTexture;
                }
            }

            Debug.LogWarning($"There is no character with given sprite index, index = {spriteIndex}", tmpText);
            return null;
        }

        public static Coroutine AnimateTextReveal(this TMP_Text tmp, string text, float perCharDelay = 0.01f,
            Action endAction = null, bool unscaled = false)
        {
            return tmp.StartCoroutine(IEAnimatedTextReveal(tmp, text, perCharDelay, endAction, unscaled));
        }

        public static IEnumerator IEAnimatedTextReveal(TMP_Text tmp, string text, float perCharDelay = 0.01f,
            Action endAction = default, bool unscaled = false)
        {
            bool richText = tmp.richText;
            tmp.richText = true;
            dynamic wfs = unscaled ? new WaitForSecondsRealtime(perCharDelay) : new WaitForSeconds(perCharDelay);
            StringBuilder sb = new(text.Length);

            for (int index = 0; index < text.Length; index++)
            {
                sb.Clear();
                sb.Append(text[..(index + 1)]);
                if (index + 1 < text.Length)
                {
                    sb.Append("<alpha=#00>");
                    sb.Append(text[(index + 1)..]);
                }

                tmp.SetText(sb);
                yield return wfs;
            }

            tmp.SetText(text);
            tmp.richText = richText;
            endAction?.Invoke();
        }
    }
}