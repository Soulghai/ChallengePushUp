using System.Collections.Generic;
using UnityEngine;

public class ColorTheme : MonoBehaviour
{
    private static int _currentTubeId;
    private static List<Color> _colorsCurrList = new List<Color>();
    

//    public static Color GetPlayerRandomColor()
//    {
//        var colorCount = 9f;
//        var ran = Random.value;
//        if (ran < 1f / colorCount) return new Color(1.0f, 105f / 255f, 182f / 255f);
//        if (ran < 2f / colorCount) return new Color(98f / 255.0f, 201f / 255f, 255f / 255f);
//        if (ran < 3f / colorCount) return new Color(203f / 255.0f, 216f / 255f, 235f / 255f);
//        if (ran < 4f / colorCount) return new Color(250f / 255.0f, 69f / 255f, 78f / 255f);
////		if (ran < 5f/colorCount) return new Color(253f / 255.0f, 141f / 255f, 38f / 255f);
//        if (ran < 5f / colorCount) return new Color(126f / 255.0f, 199f / 255f, 207f / 255f);
//        if (ran < 6f / colorCount) return new Color(47f / 255.0f, 201f / 255f, 253f / 255f);
//        if (ran < 7f / colorCount) return new Color(160f / 255.0f, 253f / 255f, 78f / 255f);
//        if (ran < 8f / colorCount) return new Color(112f / 255.0f, 149f / 255f, 252f / 255f);
//        return new Color(107f / 252.0f, 253f / 255f, 185f / 255f);
//    }


//    public static Color GetTubeRandomColor()
//    {
//        var colorCount = 9f;
//        var ran = Random.value;
//        if (ran < 1f / colorCount) return new Color(1.0f, 105f / 255f, 182f / 255f);
//        if (ran < 2f / colorCount) return new Color(98f / 255.0f, 201f / 255f, 255f / 255f);
//        if (ran < 3f / colorCount) return new Color(203f / 255.0f, 216f / 255f, 235f / 255f);
//        if (ran < 4f / colorCount) return new Color(250f / 255.0f, 69f / 255f, 78f / 255f);
////		if (ran < 5f/colorCount) return new Color(253f / 255.0f, 141f / 255f, 38f / 255f);
//        if (ran < 5f / colorCount) return new Color(126f / 255.0f, 199f / 255f, 207f / 255f);
//        if (ran < 6f / colorCount) return new Color(47f / 255.0f, 201f / 255f, 253f / 255f);
//        if (ran < 7f / colorCount) return new Color(160f / 255.0f, 253f / 255f, 78f / 255f);
//        if (ran < 8f / colorCount) return new Color(112f / 255.0f, 149f / 255f, 252f / 255f);
//        return new Color(107f / 252.0f, 253f / 255f, 185f / 255f);
//    }
    
    public static void SetFirstColor()
    {
        _currentTubeId = 0;
    }
    
    public static Color GetTubeColor()
    {
        if (_colorsCurrList.Count == 0)
        {
            InitColors();
        }
        return _colorsCurrList[_currentTubeId];
    }

    public static void GetNextRandomId()
    {
        if (_currentTubeId < _colorsCurrList.Count)
        _colorsCurrList.RemoveAt(_currentTubeId);
        // Если закончились элементы, обновляем массив
        if (_colorsCurrList.Count == 0)
        {
            InitColors();
        }
        _currentTubeId = Mathf.RoundToInt(Random.Range(0, _colorsCurrList.Count));
    }

    private static void InitColors()
    {
        _colorsCurrList = new List<Color>
        {
            new Color(227f / 255.0f, 199f / 255f, 35f / 255f),
            new Color(142f / 255.0f, 224f / 255f, 69f / 255f),
            new Color(73f / 255f, 76f / 255f, 84f / 255f),
            new Color(81f / 255f, 185f / 255f, 240f / 255f),
            new Color(199f / 255.0f, 222f / 255f, 255f / 255f),
            new Color(191f / 255.0f, 58f / 255f, 53f / 255f),
            new Color(68f / 255.0f, 218f / 255f, 251f / 255f),
            new Color(88f / 255.0f, 109f / 255f, 238f / 255f),
            new Color(37f / 252.0f, 221f / 255f, 203f / 255f)
        };
    }
}