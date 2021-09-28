using System;
namespace ParallelDI.Service
{
    public class GuidService
    {

        Guid id;

        public string CustomID { get; set; }

        public GuidService()
        {
            id = Guid.NewGuid();
        }

        public Guid GetID()
        {
            return id;
        }
    }
}
