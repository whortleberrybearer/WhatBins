////namespace WhatBins.Types.Fakes
////{
////    using Bogus;
////    using WhatBins.Types;

////    public class ExtractResultFaker : Faker<CollectionExtraction>
////    {
////        private readonly CollectionStateFaker collectionStateFaker = new CollectionStateFaker();
////        private readonly CollectionFaker collectionFaker = new CollectionFaker();

////        public ExtractResultFaker()
////        {
////            this.StrictMode(true);

////            this.CustomInstantiator(faker =>
////            {
////                CollectionState collectionState = this.collectionStateFaker.Generate();

////                if (collectionState != CollectionState.Collection)
////                {
////                    return new CollectionExtraction(collectionState);
////                }

////                return new CollectionExtraction(collectionState, this.collectionFaker.Generate(3));
////            });

////            this.AssertConfigurationIsValid();
////        }
////    }
////}
