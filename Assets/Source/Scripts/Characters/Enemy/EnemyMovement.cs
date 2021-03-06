using UnityEngine;
using Ingame.Events;
using NaughtyAttributes;
using Support;

namespace Ingame.Movement
{
    [RequireComponent(typeof(EnemyEventControl), typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {

        private const float MARGIN_ERROR_MIN = .0001f;
        private const float MARGIN_ERROR_MAX = 2f;
        private Rigidbody2D _rb;
        private EnemyEventControl enemyEventControl;

        private bool isNormalMode= true;
        private float _pushForce = 2000;

        private void Awake()
        {
            enemyEventControl = GetComponent<EnemyEventControl>();
            _rb = GetComponent<Rigidbody2D>();
        }


        private void Start()
        {
            GameController.Instance.OnFirstStagePassed += OnFirstStagePassed;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnFirstStagePassed -= OnFirstStagePassed;
        }

        private void LateUpdate()
        {
            if (isNormalMode)
            {
                FollowPlayer();
            }
            else
            {
                RunAwayFromPlayer();
            }
        }

        private void OnFirstStagePassed()
        {
            isNormalMode = false;
        }
        private void MakeMove(int i)
        {
            if (PlayerEventSystem.Instance == null)
                return;
            float dirY = (PlayerEventSystem.Instance.transform.position.y - this.transform.position.y);
            dirY = dirY > MARGIN_ERROR_MAX ? 1 : (dirY < MARGIN_ERROR_MIN ? -1 : 0);


            float dirX = (PlayerEventSystem.Instance.transform.position.x - this.transform.position.x);
            dirX = dirX > MARGIN_ERROR_MAX ? 1 : (dirX < MARGIN_ERROR_MIN ? -1 : 0);
            RotateSprite(dirX > -1);
            var direction =   Vector3.up* dirY + Vector3.right*dirX;
            direction *= enemyEventControl.EnemyStatsData.Speed*Time.fixedDeltaTime*i;
            
            _rb.velocity = direction;
        }

        public void FollowPlayer()
        {
            MakeMove(1);
        }

        public void RunAwayFromPlayer()
        {
            MakeMove(-1);
        }
        public void PushEnemy()
        {
            if (PlayerEventSystem.Instance == null)
                return;
            float dirY = (PlayerEventSystem.Instance.transform.position.y - this.transform.position.y);
            dirY = dirY > MARGIN_ERROR_MAX ? 1 : (dirY < MARGIN_ERROR_MIN ? -1 : 0);


            float dirX = (PlayerEventSystem.Instance.transform.position.x - this.transform.position.x);
            dirX = dirX > MARGIN_ERROR_MAX ? 1 : (dirX < MARGIN_ERROR_MIN ? -1 : 0);

            var direction = Vector3.up * dirY + Vector3.right * dirX;
            direction *= enemyEventControl.EnemyStatsData.Speed * Time.fixedDeltaTime*_pushForce;
            direction = -direction;
            _rb.AddForce(direction);
        }
        public void RotateSprite(bool rot)
        {
            if (rot)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }
}
