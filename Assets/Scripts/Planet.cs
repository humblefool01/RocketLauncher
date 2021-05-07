using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float _mass;
    public float _radius;

    private void OnValidate()
    {
        this.gameObject.transform.localScale = new Vector3(this._radius, this._radius, this._radius);
    }
}
