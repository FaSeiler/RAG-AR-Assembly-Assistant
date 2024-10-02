using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleButton : MonoBehaviour
{
    [Header("Start State")]
    [SerializeField]
    private Sprite startIcon;
    [SerializeField]
    private Image startBackround;

    [SerializeField]
    private UnityEvent startEvent = new UnityEvent();
    public UnityEvent StartEvent
    {
        get
        {
            return this.startEvent;
        }
    }

    [Header("Toggled State")]
    [SerializeField]
    private Sprite toggledIcon;
    [SerializeField]
    private Image toggledBackround;

    [SerializeField]
    private UnityEvent toggledEvent = new UnityEvent();
    public UnityEvent ToggledEvent
    {
        get
        {
            return this.toggledEvent;
        }
    }

    [SerializeField]
    private bool disableInteractionIfToggled = false;
    [SerializeField]
    private bool startInToggledState = false;

    [Header("On Disable")]
    [SerializeField]
    private bool resetOnDisable = false;

    [SerializeField]
    private bool resetAndInvokeToggledFunction = true;

    [SerializeField]
    private UnityEvent onDisabledEvent = new UnityEvent();
    public UnityEvent OnDisabledEvent
    {
        get
        {
            return this.onDisabledEvent;
        }
    }

    [Header("Button Type")]
    [Header("Components")]

    [SerializeField]
    private Toggle toggle;

    [Space]
    [SerializeField]
    private Image iconComponent;

    public bool isToggled = false;
    private bool isInitialized = false;

    private void Awake()
    {
        if (toggle == null)
        {
            toggle = GetComponent<Toggle>();
        }

        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }


        isInitialized = true;

        if (startInToggledState)
        {
            SetToToggledState();
        }
        else
        {
            SetToStartState();
        }
    }

    private void Start()
    {

    }

    private void OnToggleValueChanged(bool value)
    {
        PressButton();
    }

    public void StartFunction()
    {
        StartEvent?.Invoke();

        SetToToggledState();
    }

    public void ToggledFunction()
    {
        ToggledEvent?.Invoke();

        SetToStartState();
    }

    public void SetToStartState()
    {
        if (!isToggled && isInitialized)
        {
            return;
        }

        if (iconComponent)
        {
            iconComponent.sprite = startIcon;
            startBackround.gameObject.SetActive(true);
            toggledBackround.gameObject.SetActive(false);
        }

        isToggled = false;

        if (disableInteractionIfToggled)
        {
            toggle.interactable = true;
        }
    }

    public void SetToToggledState()
    {
        if (isToggled)
        {
            return;
        }

        if (iconComponent)
        {
            iconComponent.sprite = toggledIcon;
            toggledBackround.gameObject.SetActive(true);
            startBackround.gameObject.SetActive(false);
        }

        isToggled = true;

        if (disableInteractionIfToggled)
        {
            toggle.interactable = false;
        }
    }

    public void PressButton()
    {
        if (isToggled)
        {
            ToggledFunction();
        }
        else
        {
            StartFunction();
        }
    }

    private void OnDisable()
    {
        onDisabledEvent?.Invoke();

        if (resetOnDisable && isToggled)
        {
            if (resetAndInvokeToggledFunction)
            {
                ToggledFunction();
                SetToStartState();
            }
        }
    }
}