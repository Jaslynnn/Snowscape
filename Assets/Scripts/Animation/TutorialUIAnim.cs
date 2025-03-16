using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TutorialUIAnim : MonoBehaviour
{
    [System.Serializable]
    public class KeyGlowPair
    {
        public KeyCode key;        // The key to detect (e.g., A, S, D, etc.)
        public Image glowImage;    // The corresponding glow UI element
    }

    public List<KeyGlowPair> keyGlowPairs;  // Assign these in the Inspector
    private Dictionary<KeyCode, Image> glowDictionary;
    private Dictionary<KeyCode, Transform> parentDictionary;
    private Dictionary<KeyCode, Vector3> originalScales;

    private void Start()
    {
        glowDictionary = new Dictionary<KeyCode, Image>();
        parentDictionary = new Dictionary<KeyCode, Transform>();
        originalScales = new Dictionary<KeyCode, Vector3>();

        foreach (var pair in keyGlowPairs)
        {
            if (pair.glowImage != null)
            {
                glowDictionary[pair.key] = pair.glowImage;

                Transform parentObj = pair.glowImage.transform.parent; // Get parent
                if (parentObj != null)
                {
                    parentDictionary[pair.key] = parentObj;
                    originalScales[pair.key] = parentObj.localScale; // Store scale
                }

                SetGlowOpacity(pair.glowImage, 0f); // Ensure all glows start invisible
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Spacebar Pressed");
            ShowGlow(KeyCode.Space);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            //Debug.Log("Spacebar Released");
            HideGlow(KeyCode.Space);
        }
    }
    public void ShowGlow(KeyCode key)
    {
        if (glowDictionary.ContainsKey(key))
        {
            Image glow = glowDictionary[key];
            LeanTween.value(gameObject, alpha => SetGlowOpacity(glow, alpha), glow.color.a, 1f, 0.3f);
        }

        if (parentDictionary.ContainsKey(key))
        {
            Transform parent = parentDictionary[key];
            LeanTween.scale(parent.gameObject, originalScales[key] * 1.1f, 0.1f).setEaseOutQuad();
        }
    }

    public void HideGlow(KeyCode key)
    {
        if (glowDictionary.ContainsKey(key))
        {
            Image glow = glowDictionary[key];
            LeanTween.value(gameObject, alpha => SetGlowOpacity(glow, alpha), glow.color.a, 0f, 0.3f);
        }

        if (parentDictionary.ContainsKey(key))
        {
            Transform parent = parentDictionary[key];
            LeanTween.scale(parent.gameObject, originalScales[key], 0.1f).setEaseOutQuad();
        }
    }

    private void SetGlowOpacity(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}

