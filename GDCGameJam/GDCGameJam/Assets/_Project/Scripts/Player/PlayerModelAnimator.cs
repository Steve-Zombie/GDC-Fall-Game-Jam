using KBCore.Refs;
using UnityEngine;

namespace Player
{
    public class PlayerModelAnimator : MonoBehaviour
    {
        public void OnHeightChange(float height)
        {
            transform.localScale = new Vector3(transform.localScale.x, height / 2, transform.localScale.z);
            transform.localPosition = new Vector3(0, height / 2, 0);
        }
    }
}