using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenCryopreservation
{
    public class MarkToolStoragePreserves : DaySystem, IModSystem
    {
        EntityQuery ToolStorages;

        protected override void Initialise()
        {
            base.Initialise();
            ToolStorages = GetEntityQuery(typeof(CToolStorage), typeof(CHeldBy));
        }

        protected override void OnUpdate()
        {
            using NativeArray<Entity> entities = ToolStorages.ToEntityArray(Allocator.Temp);
            using NativeArray<CHeldBy> heldBys = ToolStorages.ToComponentDataArray<CHeldBy>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                CHeldBy heldBy = heldBys[i];

                if (heldBy.Holder != default && Has<CPreservesContentsOvernight>(heldBy.Holder))
                {
                    Set<CPreservesContentsOvernight>(entity);
                }
                else if (Has<CPreservesContentsOvernight>(entity))
                {
                    EntityManager.RemoveComponent<CPreservesContentsOvernight>(entity);
                }
            }
        }
    }
}
