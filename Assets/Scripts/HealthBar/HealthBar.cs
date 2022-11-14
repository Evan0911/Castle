using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Sprite heartEmpty;
    [SerializeField] private Sprite heartFull;

    List<HeartImage> heartImages;

    private void Awake()
    {
        heartImages = new List<HeartImage>();
    }

    public HeartImage CreateHeartImage(Vector2 anchoredPosition)
    {
        //Create game object
        GameObject heartGameObject = new GameObject("Heart", typeof(Image));
        //Set as a child of this transform
        heartGameObject.transform.SetParent(transform);
        heartGameObject.transform.localPosition = Vector3.zero;

        //Locate and size heart
        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);

        //Set heart Sprite
        Image HeartImageUI = heartGameObject.GetComponent<Image>();
        HeartImageUI.sprite = heartFull;

        HeartImage heartImage = new HeartImage(this, HeartImageUI);
        heartImages.Add(heartImage);

        return heartImage;
    }

    public bool Damage(HealthBarSystem system)
    {
        bool isAlive = true;
        List<HealthBarSystem.Heart> heartList = system.GetHeartList();
        for (int i = heartImages.Count - 1; i >= 0; i--)
        {
            HeartImage heartImage = heartImages[i];
            HealthBarSystem.Heart heart = heartList[i];
            heartImage.SetHeart(heart.GetState());
            isAlive = heart.GetState();
        }
        return isAlive;
    }

    public class HeartImage
    {
        private Image heartImage;
        private HealthBar healthBar;

        public HeartImage(HealthBar healthBar, Image heartImage)
        {
            this.heartImage = heartImage;
            this.healthBar = healthBar;
        }

        public void SetHeart(bool state)
        {
            switch(state)
            {
                case false: heartImage.sprite = healthBar.heartEmpty; break;
                case true: heartImage.sprite = healthBar.heartFull; break;
            }
        }
    }
}
