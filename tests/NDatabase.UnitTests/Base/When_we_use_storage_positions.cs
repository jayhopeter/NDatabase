using NDatabase.Meta;
using NUnit.Framework;

namespace NDatabase.UnitTests.Base
{
    public class When_we_use_storage_positions
    {
        [Test] 
        public void They_should_be_constants()
        {
            Assert.That(StorageEngineConstant.DatabaseHeaderVersionPosition, Is.EqualTo(0));
            Assert.That(StorageEngineConstant.DatabaseHeaderDatabaseIdPosition, Is.EqualTo(4));
            Assert.That(StorageEngineConstant.DatabaseHeaderLastTransactionId, Is.EqualTo(36));
            Assert.That(StorageEngineConstant.DatabaseHeaderNumberOfClassesPosition, Is.EqualTo(52));
            Assert.That(StorageEngineConstant.DatabaseHeaderFirstClassOid, Is.EqualTo(60));
            Assert.That(StorageEngineConstant.DatabaseHeaderLastCloseStatusPosition, Is.EqualTo(68));
            Assert.That(StorageEngineConstant.DatabaseHeaderEmptySpaceWhichCouldBeUsedInTheFuture, Is.EqualTo(69));
            Assert.That(StorageEngineConstant.DatabaseHeaderDatabaseCharacterEncodingPosition, Is.EqualTo(189));
            Assert.That(StorageEngineConstant.DatabaseHeaderCurrentIdBlockPosition, Is.EqualTo(247));
            Assert.That(StorageEngineConstant.DatabaseHeaderFirstIdBlockPosition, Is.EqualTo(255));
        }
    }
}