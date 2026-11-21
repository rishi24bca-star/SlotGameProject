
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class LeverScript : MonoBehaviour
{
    [SerializeField] ReelController reelController;
    [SerializeField] ReelController reelController2;
    [SerializeField] ReelController reelController3;
    [SerializeField] Animator animator;
    [SerializeField] private CoinCalculator _calculator;
    bool Spin = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Spin)
        {
            _calculator.CointDeduction();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(
                Input.mousePosition
            );

            Collider2D hit = Physics2D.OverlapPoint(mousePosition);

            if (hit != null && hit.gameObject == gameObject)
            {
                Debug.Log("LEVER HIT!");

                animator.ResetTrigger("Stop");
                animator.SetTrigger("Spin");

                reelController.StartSpin();
                reelController2.StartSpin();
                reelController3.StartSpin();

                Spin = true;
            }
        }
    }

    public void StopSpin()
    {
        int i = Random.Range(5, 8);
        Invoke(nameof(Delay), i);
    }

    void Delay()
    {
        animator.ResetTrigger("Spin");
        animator.SetTrigger("Stop");

        reelController.StopSpin();
        reelController2.StopSpin();
        reelController3.StopSpin();

        StartCoroutine(CheckResults());
    }

    IEnumerator CheckResults()
    {
        // Wait until all three reels have finished stopping.
        yield return new WaitUntil(() =>
            !reelController.IsSpinning &&
            !reelController2.IsSpinning &&
            !reelController3.IsSpinning
        );

        CheckWin();

        Spin = false;
    }

    void CheckWin()
    {
        string result1 = reelController.Result;
        string result2 = reelController2.Result;
        string result3 = reelController3.Result;
        
        Debug.Log("Results: " + result1 + " | " + result2 + " | " + result3);

        if (result1 == result2 && result2 == result3)
        {
            Debug.Log("YOU WIN!");
            _calculator.CoinAddition(1000);
            
        }
        else
        {
            Debug.Log("YOU LOSE!");
        }
    }
}
