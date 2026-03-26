using UnityEngine;

public class GrassManager : MonoBehaviour
{
    [Header("Grass States")]
    public float grassAmount = 10f;
    public float maxGrass = 100f;
    public bool gettingEaten = false;

    [Header("Growth Settings")]
    [SerializeField] private float grassRegenerationTime = 10f;
    [SerializeField] private float amountPerTick = 5f;

    [Header("Visuals")]
    [SerializeField] private MeshRenderer hexRenderer;
    [SerializeField] private int targetMaterialIndex = 0; 
    [SerializeField] private Color emptyColor = new Color(0.5f, 0.4f, 0.2f);
    [SerializeField] private Color fullColor = new Color(0.1f, 0.8f, 0.1f);
    [SerializeField] private Color halfColor = new Color(0.1f, 0.8f, 0.1f);

    private float timer = 0f;

    void Start()
    {
        if (hexRenderer == null)
        {
            hexRenderer = GetComponentInChildren<MeshRenderer>();
        }
        UpdateColor(grassAmount);

    }

    void Update()
    {
        if (gettingEaten || grassAmount >= maxGrass)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= grassRegenerationTime)
        {
            grassAmount += amountPerTick;
            timer = 0f;

            if (grassAmount > maxGrass)
            {
                grassAmount = maxGrass;
            }
            UpdateColor(grassAmount);

        }
    }

    public void ConsumeGrass(float eatAmount)
    {
        gettingEaten = true;

        grassAmount -= eatAmount;
        if (grassAmount < 0f) grassAmount = 0f;
        {
        UpdateColor(grassAmount);

        gettingEaten = false;
        }

        
    }

    private void UpdateColor(float grassAmount)
    {
        if (hexRenderer != null && targetMaterialIndex < hexRenderer.materials.Length)
        {
            if (grassAmount >= 80)
            {
                hexRenderer.materials[targetMaterialIndex].color=fullColor;
            }
            else if (grassAmount > 50 && grassAmount < 80)
            {
                hexRenderer.materials[targetMaterialIndex].color =halfColor;
            }
            else
            {
                hexRenderer.materials[targetMaterialIndex].color = emptyColor;
            }
        }
    }
}