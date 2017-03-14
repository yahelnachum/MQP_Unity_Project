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

        // add and set a rect transform
        RectTransform rt = newobject.GetComponent<RectTransform>();
        rt.SetParent(this.parent.GetComponent<RectTransform>());
        rt.anchorMin = new Vector2(this.nextPos.x, this.nextPos.y + this.height);
        rt.anchorMax = new Vector2(this.nextPos.x + this.width, this.nextPos.y);

        /*
        rt.position = new Vector2(this.startPos.x, this.nextPos.y);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.height);
         */

        // advance the position 
        this.advanceNextPos();

        return newobject;
    }

    private void advanceNextPos()
    {
        this.nextPos.y += this.height;
    }

}
