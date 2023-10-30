using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Shapes;

public class WandBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The SO with the list of available spells")]
    SpellBookSO _SpellBookSO;

    [SerializeField]
    [Tooltip("The prefab of the magic circle to instantiate")]
    GameObject _MagicCirclePrefab;

    [SerializeField]
    [Tooltip("The layer of the Magic Circle")]
    LayerMask _hitLayer;

    // References the instantiated magic circle
    [SerializeField] private GameObject _CurrentMagicCircleObjectReference;
    [SerializeField] private TextMeshPro _CurrentMagicCircleTextReference;
    [SerializeField] private Polyline _CurrentMagicCirclePolylineReference;

    // Used to determine if a hit on the magic circle registers as a valid spell element (no hit is registered in the central circle)
    [SerializeField] private double _MagicCircleDeadzoneRadius;

    [SerializeField] private bool _IsSpellInProgress = false;

    [SerializeField] private string _CurrentSpell = "";


    /// <summary>
    /// The function to call to start or stop the current spell of the wand.
    /// Starting a spell means spawning the magic circle and looping raycasts to register spell elements.
    /// Stopping the spell means attempting to cast the resulting spell.
    /// </summary>
    public void StartOrStopSpell()
    {
        _IsSpellInProgress = !_IsSpellInProgress;
        if (_IsSpellInProgress)
        {
            // Starts a spell
            _CurrentSpell = "";

            // Instantiates the magic circle's prefab and fixes the magic circle's x rotation at 90 to avoid it being tilted
            Quaternion adjustedRotation = Quaternion.LookRotation(GlobalReferences.Instance.Wand.position - GlobalReferences.Instance.MagicCircleSpawningPoint.position) * Quaternion.Euler(90f, 0f, 0f);
            _CurrentMagicCircleObjectReference = Instantiate(_MagicCirclePrefab, GlobalReferences.Instance.MagicCircleSpawningPoint.position, adjustedRotation, null);

            _CurrentMagicCircleTextReference = _CurrentMagicCircleObjectReference.transform.Find("Current Spell Text (TMP)").GetComponent<TextMeshPro>();
            _CurrentMagicCircleTextReference.text = _CurrentSpell;

            _CurrentMagicCirclePolylineReference = _CurrentMagicCircleObjectReference.transform.Find("RaycastImpactPolyline").GetComponent<Polyline>();
            _CurrentMagicCirclePolylineReference.SetPoints(new List<Vector3>());
        }
        else
        {
            // Ends the spell and attempt to cast it
            AttemptToCastSpell(_CurrentSpell);
            GameObject.Destroy(_CurrentMagicCircleObjectReference);
        }
    }

    public void Update()
    {
        if (_IsSpellInProgress)
        {
            // Define the starting point and direction of the ray
            Vector3 tipPosition = transform.position;   // Assuming the tip is at the center of the cylinder
            Vector3 rayDirection = transform.up;        // Assuming the tip points upwards

            RaycastHit hitInfo;

            // Perform the raycast
            if (Physics.Raycast(tipPosition, rayDirection, out hitInfo, Mathf.Infinity, _hitLayer))
            {
                Vector3 impactPoint = hitInfo.point;

                // Converts the impact point to the local coordinates of the magic circle plane
                Vector3 localImpactPoint = _CurrentMagicCircleObjectReference.transform.InverseTransformPoint(impactPoint);
                RegisterSpellCircleHit(localImpactPoint);
            }
        }
    }

    /// <summary>
    /// Registers a hit point on the spell circle, and use the local coordinates to compute which elements it corresponds to.
    /// If the element hit is not the same one as the previous, and is not the neutral dead zone, adds that new element to the spell.
    /// </summary>
    /// <param name="localImpactPoint">The raycast collision point on the magic circle, in local coordinates</param>
    private void RegisterSpellCircleHit(Vector3 localImpactPoint)
    {
        if (localImpactPoint.magnitude <= _MagicCircleDeadzoneRadius)
        {
            _CurrentMagicCirclePolylineReference.AddPoint(localImpactPoint, Color.red);
            return;     // The wand is aimed at the central dead zone of the circle : the hit is ignored
        }

        _CurrentMagicCirclePolylineReference.AddPoint(localImpactPoint, Color.yellow);
        float angle = Mathf.Atan2(localImpactPoint.x, localImpactPoint.z) * Mathf.Rad2Deg;
        RegisterSpellElementFromAngle(angle);
    }

    /// <summary>
    /// Registers the numpad notation of the element matching the angle on the magic circle.
    /// Each element zone is 1/8th of the circle's periphery.
    /// If the element hit is a different one from the previous, it is added to the spell.
    /// </summary>
    /// <param name="angle">An angle in degrees</param>
    private void RegisterSpellElementFromAngle(float angle)
    {
        char elementNumpadNotation = '-';
        if (angle > 157.5 || angle < -157.5) elementNumpadNotation = '8';
        if (angle > -157.5 && angle <= -112.5) elementNumpadNotation = '9';
        if (angle > -112.5 && angle <= -67.5) elementNumpadNotation = '6';
        if (angle > -67.5 && angle <= -22.5) elementNumpadNotation = '3';
        if (angle > -22.5 && angle <= 22.5) elementNumpadNotation = '2';
        if (angle > 22.5 && angle <= 67.5) elementNumpadNotation = '1';
        if (angle > 67.5 && angle <= 112.5) elementNumpadNotation = '4';
        if (angle > 112.5 && angle <= 157.5) elementNumpadNotation = '7';

        if (elementNumpadNotation == '-')
        {
            Debug.LogError("ANGLE COMPUTATION ERROR IN RegisterSpellElementFromAngle WITH ANGLE: " + angle);
        }

        if (_CurrentSpell.Length == 0 || _CurrentSpell[_CurrentSpell.Length - 1] != elementNumpadNotation)
        {
            // If the spell is empty, or that the element to add is not already the last inputted element, adds it and updates the spell text
            _CurrentSpell += elementNumpadNotation;
            _CurrentMagicCircleTextReference.text = _CurrentSpell;
        }
    }

    /// <summary>
    /// Casts the spell of the matching input, if there's one
    /// </summary>
    /// <param name="input">The input of the spell to cast</param>
    private void AttemptToCastSpell(string input)
    {
        SpellBookEntry matchingSpell = _SpellBookSO.getSpellObject(input);
        if (matchingSpell == null) matchingSpell = _SpellBookSO.getSpellObject("-");     // Gets a 'fizzle' spell
        if (matchingSpell == null || matchingSpell.spellObjectPrefab == null)
        {
            Debug.LogError("THE FIZZLE SPELL HAS NOT BEEN FOUND OR THE SPELL'S PREFAB IS NULL");
            return;
        }
        Instantiate(matchingSpell.spellObjectPrefab, GlobalReferences.Instance.SpellSpawningPoint.position, GlobalReferences.Instance.SpellSpawningPoint.rotation, null);
    }
}
