using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenCryopreservation
{
    public class DestroyToolsAtNight : StartOfNightSystem, IModSystem
    {
        EntityQuery Tools;
        protected override void Initialise()
        {
            base.Initialise();
            Tools = GetEntityQuery(typeof(CToolInUse), typeof(CHeldBy));
        }

        protected override void OnUpdate()
        {
            using NativeArray<Entity> entities = Tools.ToEntityArray(Allocator.Temp);
            using NativeArray<CHeldBy> heldBys = Tools.ToComponentDataArray<CHeldBy>(Allocator.Temp);

            for (int i = entities.Length - 1; i > -1; i--)
            {
                if (heldBys[i].Holder == default || !Has<CPreservesContentsOvernight>(heldBys[i].Holder))
                {
                    EntityManager.DestroyEntity(entities[i]);
                }
            }
        }
    }
}
