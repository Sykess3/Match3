using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;

namespace _Project.Code.Core.Models.BoardLogic.Spawner
{
    public interface ICellContentSpawner
    {
        void Spawn(IEnumerable<ContentToSpawn> contentToSpawn);
        void Spawn(ContentToSpawn contentToSpawn);
    }
}