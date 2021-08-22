using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _substrate;
    [SerializeField] private Image _fill;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _durationDecay = 1f;

    private Slider _slider;
    private Vector3 _localScale;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _slider.wholeNumbers = true;
        _slider.minValue = _player.MinHealth;
        _slider.maxValue = _player.MaxHealth;
        _slider.value = _player.CurrentHealth;

        _text.text = _player.CurrentHealth.ToString();

        _localScale = _substrate.GetComponent<RectTransform>().localScale;
    }

    private void OnEnable()
    {
        _player.Damaged += OnDamaged;
        _player.Cured += OnCured;
    }

    private void OnDisable()
    {
        _player.Damaged -= OnDamaged;
        _player.Cured -= OnCured;
    }

    private void TrySetHealth(int health, Color color, bool isHealth)
    {
        int direction = isHealth ? 1 : -1;
        int currentHealth = (int)_slider.value + direction * health;

        if (isHealth == true && _slider.value < _slider.maxValue || isHealth == false && _slider.value > _slider.minValue)
        {
            SetHealth(currentHealth);
            ShowSubstrate(health, color, isHealth);
        }
    }

    private void SetHealth(int health)
    {
        _slider.value = Mathf.Clamp(health, _slider.minValue, _slider.maxValue);
        _text.text = health.ToString();
    }

    private void ShowSubstrate(int health, Color color, bool isInverse = false)
    {
        RectTransform substrateRect = _substrate.GetComponent<RectTransform>();
        substrateRect.sizeDelta = new Vector2(health, substrateRect.sizeDelta.y);

        StartCoroutine(StartDecaySubstrate(color, substrateRect.sizeDelta, _durationDecay, isInverse));
    }

    private IEnumerator StartDecaySubstrate(Color color, Vector2 sizeDelta, float durationDecay, bool isInverse)
    {
        RectTransform substrateRect = _substrate.GetComponent<RectTransform>();
        RectTransform fillRect = _fill.GetComponent<RectTransform>();
        Vector3 tempPosition = fillRect.localPosition;
        float elapsedTime = 0;
        float fillOffsetX = fillRect.rect.width / 2;
        float half = 2;
        float direction = isInverse ? -1 : 1;

        _substrate.color = color;

        substrateRect.localScale = _localScale;
        substrateRect.sizeDelta = sizeDelta;

        tempPosition.x += fillOffsetX;
        substrateRect.localPosition = tempPosition;

        while (elapsedTime < durationDecay)
        {
            elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, elapsedTime, durationDecay);
            substrateRect.localScale = new Vector3(_localScale.x * (1 - elapsedTime / durationDecay), _localScale.y, _localScale.z);

            substrateRect.localPosition = new Vector3(tempPosition.x + direction * substrateRect.rect.width / half * substrateRect.localScale.x,
                                                      substrateRect.localPosition.y,
                                                      substrateRect.localPosition.z);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDamaged(int damage)
    {
        Color orange = new Color(0.8f, 0.35f, 0f);

        TrySetHealth(damage, orange, false);
    }

    private void OnCured(int health)
    {
        Color green = new Color(0f, 0.6f, 0f);

        TrySetHealth(health, green, true);
    }
}
