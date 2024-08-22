using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetter : MonoBehaviour, IData
{
    [SerializeField] private SpriteRenderer HeadRenderer, BodyRenderer,
     LeftArmRenderer, RightArmRenderer, LeftLegRenderer, RightLegRenderer;

     [SerializeField] private List<CharacterSpritesStruct> CharacterSpritesList = new List<CharacterSpritesStruct>();

     [System.Serializable]
     public struct CharacterSpritesStruct
     {
        public Sprite HeadSprite;
        public Sprite BodySprite;
        public Sprite LeftArmSprite;
        public Sprite RightArmSprite;
        public Sprite LeftLegSprite;
        public Sprite RightLegSprite;

     }

     public void LoadData(GameData data)
     {
        int equipedCharacterIndex = data.equipedCharacterIndex;

        HeadRenderer.sprite = CharacterSpritesList[equipedCharacterIndex].HeadSprite;
        BodyRenderer.sprite = CharacterSpritesList[equipedCharacterIndex].BodySprite;
        LeftArmRenderer.sprite = CharacterSpritesList[equipedCharacterIndex].LeftArmSprite;
        RightArmRenderer.sprite = CharacterSpritesList[equipedCharacterIndex].RightArmSprite;
        LeftLegRenderer.sprite = CharacterSpritesList[equipedCharacterIndex].LeftLegSprite;
        RightLegRenderer.sprite = CharacterSpritesList[equipedCharacterIndex].RightLegSprite;

     }

     public void SaveData(ref GameData data)
     {

     }

}
