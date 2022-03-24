using System;
using UnityEngine;

//enum IngredientUnit { none001, nome002, none003, none004 }

//[Serializable]
//public class Ingredient
//{
//    public string name;
//    public int amount = 1;
//    private IngredientUnit unit;
//}
//public class Recipe : MonoBehaviour
//{
//    public Ingredient potionResult;
//    public Ingredient[] potionIngredients;
//}
// ここから下が本編
//public class ScriptableObjectSample : ScriptableObject
//{
//    [SerializeField]
//    private int _sampleIntValue;
//
//    public int SampleIntValue
//    {
//        get { return _sampleIntValue; }
//
//        // エディタ内でしか呼べないようにする
//#if UNITY_EDITOR
//        set { _sampleIntValue = Mathf.Clamp(value, 0, int.MaxValue); }
//#endif
//    }
//}