using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using dotmob;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public static event Action LevelCompleted;

    [SerializeField] private float _minXDistanceBetweenHolders;
    [SerializeField] private Camera _camera;
    [SerializeField] private Holder _holderPrefab;
    [SerializeField] List<Holder> _holderPrefapList;
    [SerializeField] private Ball _ballPrefab;
    private int _holderCount=0;
    [SerializeField] private GameObject[] listHolders;
    [SerializeField] private GameObject[] listBalls;
    private List<GameObject> listBallTopRow = new List<GameObject>();

    [SerializeField] private AudioClip _winClip;

    public GameMode GameMode { get; private set; } = GameMode.Easy;
    public Level Level { get; private set; }

    private readonly List<Holder> _holders = new List<Holder>();

    private readonly Stack<MoveData> _undoStack = new Stack<MoveData>();

    public State CurrentState { get; private set; } = State.None;

    public bool HaveUndo => _undoStack.Count > 0;

    private void Awake()
    {
        Instance = this;
        var loadGameData = GameManager.LoadGameData;
        GameMode = loadGameData.GameMode;
        Level = loadGameData.Level;
        LoadLevel();
        CurrentState = State.Playing;
        _holderCount = Level.map.Count;
    }

    private void LoadLevel()
    {
        var list = PositionsForHolders(Level.map.Count, out var width).ToList();
        _camera.orthographicSize = 0.5f * width * Screen.height / Screen.width;

        for (var i = 0; i < Level.map.Count; i++)
        {
            var levelColumn = Level.map[i];
            var holder = Instantiate(GetHolderPrefap(PrefManager.GetInt("holder")), list[i], Quaternion.identity);

            holder.Init(levelColumn.Select(g =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.GroupId = g;
                return ball;
            }));

            _holders.Add(holder);
        }
    }

    public Holder GetHolderPrefap(int value)
    {
        _holderPrefab = _holderPrefapList[value];
        return _holderPrefab;
        
    }

    public void OnClickUndo()
    {
        Debug.Log("UNDO :" + _undoStack.Count);
        if(CurrentState!=State.Playing || _undoStack.Count<=0)
            return;

        var moveData = _undoStack.Pop();
        MoveBallFromOneToAnother(moveData.ToHolder,moveData.FromHolder);
    }
    public void OnClickAdd()
    {
        _holderCount++;
        List<Vector2> hold = PositionsForHolders(_holderCount, out float width).ToList();
        var holder = Instantiate(GetHolderPrefap(PrefManager.GetInt("holder")), Vector2.zero, Quaternion.identity);
        _holders.Add(holder);
        for(int i = 0;i<_holderCount;i++)
        {
            _holders[i].transform.position = hold[i];
        }

    }
    public void OnClickAddHolder()
    {
       
        
        listHolders = (GameObject.FindGameObjectsWithTag("Holder"));
        listBalls = (GameObject.FindGameObjectsWithTag("Ball"));
        if (_holderCount < 6)
        {
            
            for (int i = 0; i < listHolders.Length; i++)
            {
                listHolders[i].transform.position = new Vector2(listHolders[i].transform.position.x - 1.05f, listHolders[i].transform.position.y);
            }
            for (int i = 0; i < listBalls.Length; i++)
            {
                listBalls[i].transform.position = new Vector2(listBalls[i].transform.position.x - 1.05f, listBalls[i].transform.position.y);
            }


            var levelColumn = Enumerable.Empty<int>();
            var holder = Instantiate(GetHolderPrefap(PrefManager.GetInt("holder")), new Vector2(listHolders[_holderCount - 1].transform.position.x + _minXDistanceBetweenHolders, listHolders[_holderCount - 1].transform.position.y), Quaternion.identity);
            holder.Init(levelColumn.Select(g =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.GroupId = g;
                return ball;
            }));

            _holders.Add(holder);
            listHolders = (GameObject.FindGameObjectsWithTag("Holder"));
            _holderCount++;
            _camera.orthographicSize = 0.55f * (_holderCount) * _minXDistanceBetweenHolders * Screen.height / Screen.width;
            
        }
        else if (_holderCount == 6)
        {
            for (int i = 0; i < listHolders.Length; i++)
            {
                listHolders[i].transform.position = new Vector2(listHolders[i].transform.position.x, listHolders[i].transform.position.y + 4.67f);
            }
            for (int i = 0; i < listBalls.Length; i++)
            {
                listBalls[i].transform.position = new Vector2(listBalls[i].transform.position.x, listBalls[i].transform.position.y + 4.67f);
            }
            var levelColumn = Enumerable.Empty<int>();
            var holder = Instantiate(GetHolderPrefap(PrefManager.GetInt("holder")), new Vector2(0, -5.67f), Quaternion.identity);
            holder.Init(levelColumn.Select(g =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.GroupId = g;
                return ball;
            }));

            _holders.Add(holder);
            listHolders = (GameObject.FindGameObjectsWithTag("Holder"));
            _holderCount++;
            
        }
        else if (_holderCount < 12)
        {
            for (int i = 0; i < listBalls.Length; i++)
            {
                if (listBalls[i].transform.position.y > 0)
                {
                    listBallTopRow.Add(listBalls[i]);
                }
            }
            for (int i = 6; i < listHolders.Length; i++)
            {
                listHolders[i].transform.position = new Vector2(listHolders[i].transform.position.x - 1.05f, listHolders[i].transform.position.y);
            }
            for (int i = 0; i < listBalls.Length; i++)
            {
                if (!listBallTopRow.Contains(listBalls[i]))
                {
                    listBalls[i].transform.position = new Vector2(listBalls[i].transform.position.x - 1.05f, listBalls[i].transform.position.y);
                }
            }

            var levelColumn = Enumerable.Empty<int>();
            var holder = Instantiate(GetHolderPrefap(PrefManager.GetInt("holder")), new Vector2(listHolders[_holderCount - 1].transform.position.x + _minXDistanceBetweenHolders, listHolders[_holderCount - 1].transform.position.y), Quaternion.identity);
            holder.Init(levelColumn.Select(g =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.GroupId = g;
                return ball;
            }));

            _holders.Add(holder);
            listBallTopRow.Clear();
            listHolders = (GameObject.FindGameObjectsWithTag("Holder"));
            _holderCount++;
            
        }
        else if (_holderCount == 12)
        {
            float sizeCam = 0f;
            float sizeUp = 0f;
            if (Screen.height / Screen.width == 3840 / 2160)
            {
                sizeCam = 0.6f;
                sizeUp = 3.73f;
            }
            else
            {
                sizeCam = 0.55f;
                sizeUp = 4.67f;
            }
            for (int i = 0; i < listBalls.Length; i++)
            {
                if (listBalls[i].transform.position.y > 0)
                {
                    listBallTopRow.Add(listBalls[i]);
                }
            }
            for (int i = 0; i < listHolders.Length; i++)
            {
                if (i < 6)
                {
                    listHolders[i].transform.position = new Vector2(listHolders[i].transform.position.x, listHolders[i].transform.position.y + 2);
                }
                else
                {
                    listHolders[i].transform.position = new Vector2(listHolders[i].transform.position.x, listHolders[i].transform.position.y + sizeUp);
                }
            }
            for (int i = 0; i < listBalls.Length; i++)
            {
                if (listBallTopRow.Contains(listBalls[i]))
                {
                    listBalls[i].transform.position = new Vector2(listBalls[i].transform.position.x, listBalls[i].transform.position.y + 2);
                }
                else
                {
                    listBalls[i].transform.position = new Vector2(listBalls[i].transform.position.x, listBalls[i].transform.position.y + sizeUp);
                }
            }
            //camera
            _camera.orthographicSize = sizeCam * 6 * _minXDistanceBetweenHolders * Screen.height / Screen.width;
            var levelColumn = Enumerable.Empty<int>();
            var holder = Instantiate(GetHolderPrefap(PrefManager.GetInt("holder")), new Vector2(0, -7.67f), Quaternion.identity);
            holder.Init(levelColumn.Select(g =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.GroupId = g;
                return ball;
            }));

            _holders.Add(holder);
            listBallTopRow.Clear();
            listHolders = (GameObject.FindGameObjectsWithTag("Holder"));
            _holderCount++;
            
        }
        else if (_holderCount > 12 && _holderCount < 18)
        {
            for (int i = 0; i < listBalls.Length; i++)
            {
                if (listBalls[i].transform.position.y >= -1)
                {
                    listBallTopRow.Add(listBalls[i]);
                }
            }
            for (int i = 12; i < listHolders.Length; i++)
            {
                listHolders[i].transform.position = new Vector2(listHolders[i].transform.position.x - 1.05f, listHolders[i].transform.position.y);
            }
            for (int i = 0; i < listBalls.Length; i++)
            {
                if (!listBallTopRow.Contains(listBalls[i]))
                {
                    listBalls[i].transform.position = new Vector2(listBalls[i].transform.position.x - 1.05f, listBalls[i].transform.position.y);
                }
            }
            //camera
            if (Screen.height / Screen.width == 3840 / 2160)
            {
                _camera.orthographicSize = 0.6f * 6 * _minXDistanceBetweenHolders * Screen.height / Screen.width;
            }
            else
            {
                _camera.orthographicSize = 0.55f * 6 * _minXDistanceBetweenHolders * Screen.height / Screen.width;
            }
            var levelColumn = Enumerable.Empty<int>();
            var holder = Instantiate(GetHolderPrefap(PrefManager.GetInt("holder")), new Vector2(listHolders[_holderCount - 1].transform.position.x + _minXDistanceBetweenHolders, listHolders[_holderCount - 1].transform.position.y), Quaternion.identity);
            holder.Init(levelColumn.Select(g =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.GroupId = g;
                return ball;
            }));

            _holders.Add(holder);
            listBallTopRow.Clear();
            listHolders = (GameObject.FindGameObjectsWithTag("Holder"));
            _holderCount++;
        }
    }

    private void Update()
    {

        if(CurrentState != State.Playing)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var collider = Physics2D.OverlapPoint(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (collider != null)
            {
                var holder = collider.GetComponent<Holder>();

                if (holder != null)
                    OnClickHolder(holder);
            }
        }
    }

    private void OnClickHolder(Holder holder)
    {
        var pendingHolder = _holders.FirstOrDefault(h => h.IsPending);

        if (pendingHolder != null && pendingHolder != holder)
        {
            if (holder.TopBall == null || (pendingHolder.TopBall.GroupId == holder.TopBall.GroupId && !holder.IsFull))
            {
                _undoStack.Push(new MoveData
                {
                    FromHolder = pendingHolder,
                    ToHolder = holder,
                    Ball = pendingHolder.TopBall
                });
                MoveBallFromOneToAnother(pendingHolder,holder);

            }
            else
            {
                pendingHolder.IsPending = false;
                holder.IsPending = true;
            }
        }
        else
        {
            if (holder.Balls.Any())
                holder.IsPending = !holder.IsPending;
        }
    }

    private void MoveBallFromOneToAnother(Holder fromHolder,Holder toHolder)
    {
        toHolder.Move(fromHolder.RemoveTopBall());
        CheckAndGameOver();
    }

    private void CheckAndGameOver()
    {
        if (_holders.All(holder =>
        {
            var balls = holder.Balls.ToList();
            return balls.Count == 0 || balls.All(ball => ball.GroupId == balls.First().GroupId);
        }) && _holders.Where(holder => holder.Balls.Any()).GroupBy(holder => holder.Balls.First().GroupId).All(holders => holders.Count()==1)) 
        {
            OverTheGame();
        }
    }

    private void OverTheGame()
    {
        if(CurrentState!=State.Playing)
            return;

        PlayClipIfCan(_winClip);
        CurrentState = State.Over;
      
        ResourceManager.CompleteLevel(GameMode,Level.no);
        LevelCompleted?.Invoke();
    }

    private void PlayClipIfCan(AudioClip clip,float volume=0.35f)
    {
        if(!AudioManager.IsSoundEnable || clip ==null)
            return;
        AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position,volume);
    }

    public IEnumerable<Vector2> PositionsForHolders(int count, out float expectWidth)
    {
        expectWidth = 4 * _minXDistanceBetweenHolders;
        if (count <= 6)
        {
            var minPoint = transform.position - ((count - 1) / 2f) * _minXDistanceBetweenHolders * Vector3.right - Vector3.up*1f;

            expectWidth = Mathf.Max(count * _minXDistanceBetweenHolders, expectWidth);

            return Enumerable.Range(0, count)
                .Select(i => (Vector2) minPoint + i * _minXDistanceBetweenHolders * Vector2.right);
        }

        var aspect = (float) Screen.width / Screen.height;

        var maxCountInRow = Mathf.CeilToInt(count / 2f);

        if ((maxCountInRow + 1) * _minXDistanceBetweenHolders > expectWidth)
        {
            expectWidth = (maxCountInRow + 1) * _minXDistanceBetweenHolders;
        }

        var height = expectWidth / aspect;


        var list = new List<Vector2>();
        var topRowMinPoint = transform.position + Vector3.up * height / 6f -
                             ((maxCountInRow - 1) / 2f) * _minXDistanceBetweenHolders * Vector3.right - Vector3.up*1f;
        list.AddRange(Enumerable.Range(0, maxCountInRow)
            .Select(i => (Vector2) topRowMinPoint + i * _minXDistanceBetweenHolders * Vector2.right));

        var lowRowMinPoint = transform.position - Vector3.up * height / 6f -
                             (((count - maxCountInRow) - 1) / 2f) * _minXDistanceBetweenHolders * Vector3.right - Vector3.up * 1f;
        list.AddRange(Enumerable.Range(0, count - maxCountInRow)
            .Select(i => (Vector2) lowRowMinPoint + i * _minXDistanceBetweenHolders * Vector2.right));

        return list;
    }


    public enum State
    {
        None,Playing,Over
    }

    public struct MoveData
    {
        public Holder FromHolder { get; set; }
        public Holder ToHolder { get; set; }
        public Ball Ball { get; set; }
    }
}

[Serializable]
public struct LevelGroup:IEnumerable<Level>
{
    public List<Level> levels;
    public IEnumerator<Level> GetEnumerator()
    {
        return levels?.GetEnumerator() ?? Enumerable.Empty<Level>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

[Serializable]
public struct Level
{
    public int no;
    public List<LevelColumn> map;
}

[Serializable]
public struct LevelColumn : IEnumerable<int>
{
    public List<int> values;

    public IEnumerator<int> GetEnumerator()
    {
        return values?.GetEnumerator() ?? Enumerable.Empty<int>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}