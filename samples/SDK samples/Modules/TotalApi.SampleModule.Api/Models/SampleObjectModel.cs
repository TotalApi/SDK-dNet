using System.Runtime.Serialization;
using TotalApi.Core;
using TotalApi.Core.Api;

namespace TotalApi.SampleModule.Api.Models
{
    [DataContract]
    [ClassDescription(typeof(ISampleDbContext))]
    public class SampleObjectModel: EntityObject<SampleObjectModel>,  IEntityObjectId
    {
        public string Id {
            get { return SampleObjectId; }
            set { SampleObjectId = value; }
        }

        [DataMember]
        public string SampleObjectId { get; set; }

        [DataMember]
        public int TestProp { get; set; }
    }

    public interface ISampleDbContext: IDbDataContext
    {
        //declare additional methods or properties here
    }

    public interface ISampleObjectModel : IRepositoryId<SampleObjectModel>
    {
        //declare additional methods or properties here

        int SomeAdditionalProperty { get; set; }
    }

}
