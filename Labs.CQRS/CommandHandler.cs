using Labs.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{
    public class CommandHandler<T> where T : AggregateRoot<T>, new()
    {
        private readonly IRepository<T> _repository;

        public CommandHandler(IRepository<T> repository)
        {
            _repository = repository;
        }

        public void Create(CreateItemCommand message)
        {
            var item = new T();
            item.Id = message.Id;
            _repository.Save(item, -1);
        }

        public void Delete(DeleteItemCommand message)
        {
            var item = new T();
            item.Id = message.Id;
            _repository.Save(item, -1);
        }

    }
}
