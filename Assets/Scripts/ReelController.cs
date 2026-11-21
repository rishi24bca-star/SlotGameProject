
using UnityEngine;

public class ReelController : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 3.5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private int extraRounds = 2;
  
    [Header("Symbol Settings")]
    [SerializeField] private float symbolSpacing = 1f;
    [SerializeField] private float centerY = 1f;
    private static int Amount;
    private Transform[] symbols;

    private float currentSpeed;
    private float totalLoopLength;
    private float bottomBoundary;

    private bool isSpinning;
    private bool isStopping;

    private Transform winningSymbol;
    private float remainingDistance;

    // Accessible from LeverScript
    public string Result { get; private set; }
    public bool IsSpinning => isSpinning;

    public void Initialize(Transform reelPoint)
    {
        int count = reelPoint.childCount;

        symbols = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            symbols[i] = reelPoint.GetChild(i);
        }

        totalLoopLength = count * symbolSpacing;
        bottomBoundary = -symbolSpacing;

        currentSpeed = 0f;
        isSpinning = false;
        isStopping = false;

        Result = "";
        winningSymbol = null;
    }

    private void Update()
    {
        if (symbols == null || symbols.Length == 0)
            return;

        if (!isSpinning)
            return;

        if (isStopping)
        {
            UpdateStopping();
        }
        else
        {
            UpdateSpinning();
        }
    }

    public void StartSpin()
    {
        if (isSpinning)
            return;

        Result = "";

        isSpinning = true;
        isStopping = false;

        currentSpeed = 0f;
        winningSymbol = null;
    }

    private void UpdateSpinning()
    {
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            spinSpeed,
            acceleration * Time.deltaTime
        );

        MoveReel(currentSpeed * Time.deltaTime);
    }

    public void StopSpin()
    {
        if (!isSpinning || isStopping)
            return;

        // Randomly select the symbol before stopping.
        int randomIndex = Random.Range(0, symbols.Length);
        winningSymbol = symbols[randomIndex];

        float distanceToCenter = Mathf.Repeat(
            winningSymbol.localPosition.y - centerY,
            totalLoopLength
        );

        remainingDistance = distanceToCenter
                            + (extraRounds * totalLoopLength);

        float brakingDistance =
            (currentSpeed * currentSpeed) / (2f * deceleration);

        while (remainingDistance < brakingDistance + 0.1f)
        {
            remainingDistance += totalLoopLength;
        }

        isStopping = true;
    }

    private void UpdateStopping()
    {
        if (remainingDistance <= 0.001f)
        {
            FinishStopping();
            return;
        }

        float brakingDistance =
            (currentSpeed * currentSpeed) / (2f * deceleration);

        if (remainingDistance <= brakingDistance)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }

        float distanceThisFrame = Mathf.Min(
            currentSpeed * Time.deltaTime,
            remainingDistance
        );

        MoveReel(distanceThisFrame);

        remainingDistance -= distanceThisFrame;

        if (remainingDistance <= 0.001f)
        {
            FinishStopping();
        }
    }

    private void MoveReel(float distance)
    {
        foreach (Transform symbol in symbols)
        {
            Vector3 position = symbol.localPosition;

            position.y -= distance;

            while (position.y < bottomBoundary)
            {
                position.y += totalLoopLength;
            }

            symbol.localPosition = position;
        }
    }

    private void FinishStopping()
    {
        if (winningSymbol == null)
            return;

        float correction =
            centerY - winningSymbol.localPosition.y;

        foreach (Transform symbol in symbols)
        {
            Vector3 position = symbol.localPosition;

            position.y += correction;

            symbol.localPosition = position;
        }

        // Save the final result.
        Result = winningSymbol.name.Replace("(Clone)", "").Trim();
        
       
        Debug.Log("Reel result: " + Result);

        currentSpeed = 0f;
        remainingDistance = 0f;

        isSpinning = false;
        isStopping = false;
    }
}
