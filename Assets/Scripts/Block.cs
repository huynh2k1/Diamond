using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static Block instance;

    public SpriteRenderer spriteRenderer;
    public ColorType colorType;

    public int row;
    public int col;

    private void OnMouseDown()
    {
        instance = this;
        BoardController board = BoardController.instance;
        GameController gameController = GameController.instance;

        if (gameController.canClick == true)
        {
            if (board.grid[instance.row, instance.col].spriteRenderer.sprite != null)
            {
                board.listBlockCanEat.Clear();
                board.DFS(instance.row, instance.col);
            
                if (board.listBlockCanEat.Count > 1)
                {
                    foreach (Block block in board.listBlockCanEat)
                    {
                        block.UpdateSprite(null);
                    }
                    StartCoroutine(board.TranslateRow());
                    //board.IsHasACouple();
                    //StartCoroutine(board.UpdatePoint());
                }
            }
            gameController.canClick = false;
            StartCoroutine(board.WaitUntilCanClick());
        }
    }

    public void UpdateSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
    public void UpdateTypeColor(ColorType type)
    {
        colorType = type;

    }
    public void Falling(float destination, float duration, float delay)
    {
        transform.DOKill();
        transform.DOMoveY(destination, duration).SetEase(Ease.OutBounce).SetDelay(delay);
    }

}
