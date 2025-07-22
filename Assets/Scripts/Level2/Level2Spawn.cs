using Core;

namespace Level2
{
    public class Level2Spawn : BaseSpawner
    {
        public override void Initialization()
        {
            SpawnAllItems();
        }

        private void SpawnAllItems()
        {
            for (var i = 0; i < startSpawnPositions.Count; i++)
            {
                if (i >= Level2Global.instance.allItems.Count) break;
                var newItem = Instantiate(Level2Global.instance.allItems[i], parent, false);
                newItem.name = Level2Global.instance.allItems[i].name;
                var moveItem = newItem.GetComponent<MoveItem>();
                if (moveItem)
                {
                    moveItem.Initialization(startSpawnPositions[i].transform.position, endSpawnPositions[i].transform.position);
                    StartCoroutine(moveItem.Move());
                }

                activeItem.Add(newItem);
            }
        }
    }
}