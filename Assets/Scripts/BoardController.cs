using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;

    public int row, column;
    public Block blockPref;
    public Block[,] grid;
    public List<Block> listBlock;
    public BlockSO blockSO;
    public List<Block> listBlockCanEat;

    public bool[,] visited = new bool[10, 10];
    public int[] dx = { -1, 0, 0, 1 };
    public int[] dy = { 0, -1, 1, 0 };

    public float speedFall;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitBoard();
    }

    private void InitBoard(){
        grid = new Block[row, column];
        for(int i = 0; i < row; i++)
        {
            //if(i == 0 || i == row - 1){
            //    for(int j = 1; j < column - 1; j++)
            //    {
            //        Vector2 pos = new Vector2(j, i);
            //        Block block = Instantiate(blockPref, pos, Quaternion.identity);
            //        grid[i, j] = block;
            //        block.transform.SetParent(transform, false);
            //        block.name = "(" + i + "_" + j + ")";
            //        listBlock.Add(block);

            //        int index = UnityEngine.Random.Range(0, blockSO.listBlockData.Length);

            //        block.UpdateTypeColor(blockSO.listBlockData[index].colorType);
            //        block.UpdateSprite(blockSO.listBlockData[index].sprite);

            //        block.row = i;
            //        block.col = j;
            //    }
            //}
            //else
            //{
                for(int j = 0; j < column; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    Block block = Instantiate(blockPref, pos, Quaternion.identity);
                    grid[i, j] = block;
                    block.transform.SetParent(transform, false);
                    block.name = "(" + i + "_" + j + ")";
                    listBlock.Add(block);

                    int index = UnityEngine.Random.Range(0, blockSO.listBlockData.Length);

                    block.UpdateTypeColor(blockSO.listBlockData[index].colorType);
                    block.UpdateSprite(blockSO.listBlockData[index].sprite);

                    block.row = i;
                    block.col = j;
                }
            //}
        }
    }

    public void DFS(int i, int j)
    {
        visited[i, j] = true;
        ColorType type = grid[i, j].colorType;
        for(int k = 0; k < 4; k++)
        {
            int i1 = i + dx[k];
            int j1 = j + dy[k];

            if(i1 >= 0 && i1 < row && j1 >= 0 && j1 < column
            && !visited[i1, j1] && grid[i1, j1].colorType == type 
            && grid[i1, j1].spriteRenderer.sprite != null)
            {
                DFS(i1, j1);
            }
        }
        listBlockCanEat.Add(grid[i, j]);
    }

    public void DFS_2(int i, int j, ref int value)
    {
        visited[i, j] = true;

        for (int k = 0; k < 4; k++)
        {
            int i1 = i + dx[k];
            int j1 = j + dy[k];
            if (i1 >= 0 && i1 < row && j1 >= 0 && j1 < column && grid[i1, j1].spriteRenderer.sprite == grid[i, j].spriteRenderer.sprite && !visited[i1, j1])
            {
                DFS_2(i1, j1, ref value);
            }
        }


        value += 1;
    }

    public IEnumerator TranslateRow()
    {
        Debug.Log("111111111");
        float delay = 0.02f;
        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < column; i++)
            {
                if (grid[i, j].spriteRenderer.sprite == null)
                {
                    SwapSpriteRow(i, j, delay);
                    delay += 0.02f;
                }
            }
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < column; i++)
        {
            if (grid[i, 0].spriteRenderer.sprite == null)
            {
                SwapSpriteColumn(i);
            }
        }

        //if (count1 > 0 || count2 > 0)
        //{
        //    SoundController.instance.PlayAudioClipByIndex(3, 1f);
        //    count1 = 0;
        //    count2 = 0;
        //}
        ResetVisited();

    }

    //Kiểm tra còn tồn tại cặp nào không
    public bool IsHasACouple()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (grid[i, j].spriteRenderer.sprite != null)
                {
                    int value = 0;
                    DFS_2(i, j, ref value);
                    ResetVisited();
                    if (value > 1)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void ResetVisited()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                visited[i, j] = false;
            }
        }
    }

    // Dịch các ô theo hàng
    public void SwapSpriteRow(int i, int j, float delay)
    {

        for (int k = j; k < column; k++)
        {
            if (grid[i, k].spriteRenderer.sprite != null) // nếu ô ở cột thứ i, hàng k != null
            {
                //count1++;
                ColorType type = grid[i, j].colorType;
                Sprite temp = grid[i, j].spriteRenderer.sprite;

                grid[i, j].colorType = grid[i, k].colorType;
                grid[i, j].spriteRenderer.sprite = grid[i, k].spriteRenderer.sprite;

                grid[i, k].colorType = type;
                grid[i, k].spriteRenderer.sprite = temp;
                StartCoroutine(FallByRow(i, j, k, delay));
                return;
            }
        }
    }

    public void SwapSpriteColumn(int i)
    {
        for (int k = i; k < row; k++)
        {
            if (grid[k, 0].spriteRenderer.sprite != null)
            {
                for (int j = 0; j < column; j++)
                {
                    if (grid[k, j].spriteRenderer.sprite == null)
                    {
                        return;
                    }
                    else
                    {
                        //count2++;
                        Sprite temp = grid[i, j].spriteRenderer.sprite;
                        grid[i, j].spriteRenderer.sprite = grid[k, j].spriteRenderer.sprite;
                        grid[k, j].spriteRenderer.sprite = temp;

                        ColorType type = grid[i, j].colorType;
                        grid[i, j].colorType = grid[k, j].colorType;
                        grid[k, j].colorType = type;
                    }
                }
                return;
            }
        }
    }

    //Rơi theo hàng
    IEnumerator FallByRow(int row, int col, int oldCol, float delay)
    {
        foreach (Block block in listBlock)
        {
            if (block.row == row && block.col == col)
            {
                float target = block.transform.position.y;
                block.transform.position = GetBlock(row, oldCol);
                yield return new WaitForSeconds(0.1f);
                block.Falling(target, speedFall, delay);

            }
        }
    }

    public Vector3 GetBlock(int row, int col)
    {
        foreach (Block block in listBlock)
        {
            if (block.row == row && block.col == col)
            {
                return block.transform.position;
            }
        }
        return Vector3.zero;
    }

    public void Falling(float destination, float duration, float delay)
    {
        transform.DOKill();
        transform.DOMoveY(destination, duration).SetEase(Ease.OutBounce).SetDelay(delay);
    }
    public IEnumerator WaitUntilCanClick()
    {
        yield return StartCoroutine(TranslateRow());
        GameController.instance.CheckWin();

        GameController.instance.canClick = true;
    }
}
