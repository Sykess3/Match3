using System;
using System.Collections.Generic;

namespace _Project.Code.Core.Models.DataStructures
{
    public abstract class ObjectPool<TArg, TReturn> where TReturn : IPoolItem<TArg>
    {
        private readonly IObjectPoolFactory<TArg, TReturn> _factory;
        private readonly Dictionary<TArg, Queue<TReturn>> _content;

        protected  ObjectPool(IObjectPoolFactory<TArg, TReturn> factory)
        {
            _factory = factory;
            _content = new Dictionary<TArg, Queue<TReturn>>();
        }

        public TReturn Get(TArg type)
        {
            if (!_content.ContainsKey(type))
                _content.Add(type, new Queue<TReturn>());
            if (_content[type].Count == 0)
                AddObjects(type, 1);

            var objectPoolItem = _content[type].Dequeue();
            objectPoolItem.Enable();
            return objectPoolItem;
        }

        private void ReturnToPool(TReturn itemToReturn) =>
            _content[itemToReturn.MatchType].Enqueue(itemToReturn);

        private void AddObjects(TArg type, int count)
        {
            for (int i = 0; i < count; i++)
            {
                TReturn item = _factory.Create(type);
                item.Disabled += OnDisabled;
                _content[type].Enqueue(item);
            }
        }

        private void OnDisabled(object sender, EventArgs e)
        {
            var itemToReturn = (TReturn) sender;
            itemToReturn.Disabled -= OnDisabled;
            ReturnToPool(itemToReturn);
        }

    }
}