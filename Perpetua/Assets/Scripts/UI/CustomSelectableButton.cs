using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
//https://stackoverflow.com/questions/48075615/unity-use-automatic-navigation-in-explicit-navigation
//Based on Liolik's answer
namespace Editor
{
    // Custom navigation - uses set explicit options unless they aren't set, in that case relies on automatic
    // Should be set to explicit
    public class CustomSelectableButton : UnityEngine.UI.Button
    {
        public override Selectable FindSelectableOnUp()
        {
            return navigation.selectOnUp != null ? navigation.selectOnUp : FindSelectable(transform.rotation * Vector3.up);
        }

        public override Selectable FindSelectableOnDown()
        {
            return navigation.selectOnDown != null ? navigation.selectOnDown : FindSelectable(transform.rotation * Vector3.down);
            //return navigation.selectOnDown != null ? navigation.selectOnDown : FindSelectable((transform.parent.position - new Vector3(0f, 3f, 0f)) - transform.position);
        }

        public override Selectable FindSelectableOnLeft()
        {
            return navigation.selectOnLeft != null ? navigation.selectOnLeft : FindSelectable(transform.rotation * Vector3.left);
        }

        public override Selectable FindSelectableOnRight()
        {
            return navigation.selectOnRight != null ? navigation.selectOnRight : FindSelectable(transform.rotation * Vector3.right);
        }
    }
}