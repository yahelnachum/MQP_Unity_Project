using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextSpawner
{

    private readonly Vector2 startPos;
    private readonly float height;
    private readonly float width;
    private readonly GameObject parent;

    private Vector2 nextPos;
    private int counter = 0;

    /// <summary>
    /// Returns a new TextSpawner with the given values. The start position is defaulted to 0, 0.
    /// </summary>
    /// <param name="height">Height of text elements</param>
    /// <param name="parent">Parent object of the elements to spawn</param>
    public TextSpawner(float height, float width, GameObject parent) :
        this(0f, 0f, height, width, parent) { }

    /// <summary>
    /// Returns a new TextSpawner with the given values
    /// </summary>
    /// <param name="x">X-coordinate of the desired starting position</param>
    /// <param name="y">Y-coordinate of the desired starting position</param>
    /// <param name="height">Height of text elements</param>
    /// <param name="parent">Parent object of the elements to spawn</param>
    public TextSpawner(float x, float y, float height, float width, GameObject parent) :
        this(new Vector2(x, y), height, width, parent) { }

    /// <summary>
    /// Returns a new TextSpawner with the given values
    /// </summary>
    /// <param name="startpos">Desired starting position</param>
    /// <param name="height">Height of text elements</param>
    /// <param name="parent">Parent object of the elements to spawn</param>
    public TextSpawner(Vector2 startpos, float height, float width, GameObject parent)
    {
        this.startPos = startpos;
        this.height = height;
        this.width = width;
        this.parent = parent;
        this.nextPos = new Vector2(this.startPos.x, this.startPos.y);
    }

    public GameObject spawnText(string text)
    {
        // create the new object
        GameObject newobject = new GameObject("chat_element_" + this.counter++);

        // add and set a text component
        Text textcomp = newobject.AddComponent<Text>();
        textcomp.text = text;
        textcomp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textcomp.color = Color.black;
        textcomp.fontSize = 100;

        // add and set a rect transform
        RectTransform rt = newobject.GetComponent<RectTransform>();
        rt.SetParent(this.parent.GetComponent<RectTransform>());
        rt.localPosition = new Vector3(1f, 1f, 0f);
        rt.pivot = new Vector3(1f, 1f, 1f);
        rt.anchorMin = new Vector3(0f, 1f, 0f);
        rt.anchorMax = new Vector3(1f, 1f, 0f);
        rt.offsetMax = new Vector3(0f, -this.nextPos.y, 0f);
        rt.sizeDelta = new Vector3(1f, this.height, 0f);
        rt.localScale = new Vector3(1f, 1f, 1f);

        // advance the position 
        this.advanceNextPos();

        return newobject;
    }

    private void advanceNextPos()
    {
        this.nextPos.y += this.height;
    }

}
