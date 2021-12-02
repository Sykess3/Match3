using System.Collections;
using _Project.Code.Core.Models.BoardLogic.Cells;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    public class CellContentView : View
    {
        //TODO: REMOVE THIS FOR DEBUG
        public CellContent CellContent;

        [SerializeField] private ParticleSystem _matchVFX;

        public void Match()
        {
            Instantiate(_matchVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}