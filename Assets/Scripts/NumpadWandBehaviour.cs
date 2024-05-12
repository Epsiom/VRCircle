using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NumpadWandBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The SO with the list of available spells")]
    private SpellBookSO _spellBookSO;

    [SerializeField] private Transform _spellSpawningPoint;

    // The input actions matching the keypad
    [SerializeField] private InputActionReference _numpad0;
    [SerializeField] private InputActionReference _numpad1;
    [SerializeField] private InputActionReference _numpad2;
    [SerializeField] private InputActionReference _numpad3;
    [SerializeField] private InputActionReference _numpad4;
    [SerializeField] private InputActionReference _numpad5;
    [SerializeField] private InputActionReference _numpad6;
    [SerializeField] private InputActionReference _numpad7;
    [SerializeField] private InputActionReference _numpad8;
    [SerializeField] private InputActionReference _numpad9;

    [SerializeField] private TextMeshPro _currentSpellTextReference;
    
    private bool _isSpellInProgress = false;
    private string _currentSpell = "";

    private void Start()
    {
        _numpad5.action.Enable();
        _numpad5.action.performed += StartOrStopSpell;

        _numpad1.action.Enable();
        _numpad1.action.performed += ctx => AddElement("1");
        _numpad2.action.Enable();
        _numpad2.action.performed += ctx => AddElement("2");
        _numpad3.action.Enable();
        _numpad3.action.performed += ctx => AddElement("3");
        _numpad4.action.Enable();
        _numpad4.action.performed += ctx => AddElement("4");
        _numpad6.action.Enable();
        _numpad6.action.performed += ctx => AddElement("6");
        _numpad7.action.Enable();
        _numpad7.action.performed += ctx => AddElement("7");
        _numpad8.action.Enable();
        _numpad8.action.performed += ctx => AddElement("8");
        _numpad9.action.Enable();
        _numpad9.action.performed += ctx => AddElement("9");
    }

    private void StartOrStopSpell(InputAction.CallbackContext obj)
    {
        _isSpellInProgress = !_isSpellInProgress;
        if (_isSpellInProgress)
        {
            // Starts a spell
            _currentSpell = "";
            _currentSpellTextReference.text = _currentSpell;
        }
        else
        {
            // Ends the spell and attempt to cast it
            AttemptToCastSpell(_currentSpell);
        }
    }

    private void AddElement(string elementNum)
    {
        if (_isSpellInProgress)
        {
            _currentSpell += elementNum;
            _currentSpellTextReference.text = _currentSpell;
            Debug.Log(_currentSpell);
        }
    }

    /// <summary>
    /// Casts the spell of the matching input, if there's one
    /// </summary>
    /// <param name="input">The input of the spell to cast</param>
    private void AttemptToCastSpell(string input)
    {
        Debug.Log("Numpad spell : changing the global positions to the ones of the nupad wand");
        GlobalReferences.Instance.Wand = this.gameObject.transform;
        GlobalReferences.Instance.SpellSpawningPoint = _spellSpawningPoint;

        SpellBookEntry spellBookEntry = _spellBookSO.getSpellObject(input);  // First, check if the spell is in the spellbook

        // The spell is in the spellbook
        if (spellBookEntry != null)
        {
            Instantiate(spellBookEntry.spellObjectPrefab, GlobalReferences.Instance.SpellSpawningPoint.position, GlobalReferences.Instance.SpellSpawningPoint.rotation, null);
            return;
        }
        else
        {
            GameObject modularSpell = SpellParser.ParseAndBuildSpell(input);   // Second, since the spell isn't in the spellbook, check and instantiate the modular spell

            if (modularSpell == null)                                          // Third, if the modular spell is null, it wasn't instantiated, and we instantiate manually a fizzle spell instead
            {
                SpellBookEntry fizzleSpellBookEntry = _spellBookSO.getSpellObject("-");     // Gets a 'fizzle' spell
                Instantiate(fizzleSpellBookEntry.spellObjectPrefab, GlobalReferences.Instance.SpellSpawningPoint.position, GlobalReferences.Instance.SpellSpawningPoint.rotation, null);
            }
        }
    }
}
