﻿using System.Collections.Generic;

namespace Play.Core.Iso8825.Tests.Ber.TagTests.Tags.TestData
{
    public class TagTestValues
    {
        #region Static Metadata

        public static readonly List<TagTestValue> ValidTags = new()
        {
            new TagTestValue(new byte[] {0x9F, 0x01}),
            new TagTestValue(new byte[] {0x9F, 0x40}),
            new TagTestValue(new byte[] {0x81}),
            new TagTestValue(new byte[] {0x9F, 0x02}),
            new TagTestValue(new byte[] {0x9F, 0x04}),
            new TagTestValue(new byte[] {0x9F, 0x03}),
            new TagTestValue(new byte[] {0x9F, 0x3A}),
            new TagTestValue(new byte[] {0x9F, 0x26}),
            new TagTestValue(new byte[] {0x9F, 0x42}),
            new TagTestValue(new byte[] {0x9F, 0x44}),
            new TagTestValue(new byte[] {0x9F, 0x05}),
            new TagTestValue(new byte[] {0x5F, 0x25}),
            new TagTestValue(new byte[] {0x5F, 0x24}),
            new TagTestValue(new byte[] {0x94}),
            new TagTestValue(new byte[] {0x4F}),
            new TagTestValue(new byte[] {0x9F, 0x06}),
            new TagTestValue(new byte[] {0x82}),
            new TagTestValue(new byte[] {0x50}),
            new TagTestValue(new byte[] {0x9F, 0x12}),
            new TagTestValue(new byte[] {0x5A}),
            new TagTestValue(new byte[] {0x5F, 0x34}),
            new TagTestValue(new byte[] {0x87}),
            new TagTestValue(new byte[] {0x9F, 0x3B}),
            new TagTestValue(new byte[] {0x9F, 0x43}),
            new TagTestValue(new byte[] {0x61}),
            new TagTestValue(new byte[] {0x9F, 0x36}),
            new TagTestValue(new byte[] {0x9F, 0x07}),
            new TagTestValue(new byte[] {0x9F, 0x08}),
            new TagTestValue(new byte[] {0x9F, 0x09}),
            new TagTestValue(new byte[] {0x89}),
            new TagTestValue(new byte[] {0x8A}),
            new TagTestValue(new byte[] {0x5F, 0x54}),
            new TagTestValue(new byte[] {0x8C}),
            new TagTestValue(new byte[] {0x8D}),
            new TagTestValue(new byte[] {0x5F, 0x20}),
            new TagTestValue(new byte[] {0x9F, 0x0B}),
            new TagTestValue(new byte[] {0x8E}),
            new TagTestValue(new byte[] {0x9F, 0x34}),
            new TagTestValue(new byte[] {0x8F}),
            new TagTestValue(new byte[] {0x9F, 0x22}),
            new TagTestValue(new byte[] {0x83}),
            new TagTestValue(new byte[] {0x9F, 0x27}),
            new TagTestValue(new byte[] {0x9F, 0x45}),
            new TagTestValue(new byte[] {0x84}),
            new TagTestValue(new byte[] {0x9D}),
            new TagTestValue(new byte[] {0x73}),
            new TagTestValue(new byte[] {0x9F, 0x49}),
            new TagTestValue(new byte[] {0x70}),
            new TagTestValue(new byte[] {0xBF, 0x0C}),
            new TagTestValue(new byte[] {0xA5}),
            new TagTestValue(new byte[] {0x6F}),
            new TagTestValue(new byte[] {0x9F, 0x4C}),
            new TagTestValue(new byte[] {0x9F, 0x2D}),
            new TagTestValue(new byte[] {0x9F, 0x2E}),
            new TagTestValue(new byte[] {0x9F, 0x2F}),
            new TagTestValue(new byte[] {0x9F, 0x46}),
            new TagTestValue(new byte[] {0x9F, 0x47}),
            new TagTestValue(new byte[] {0x9F, 0x48}),
            new TagTestValue(new byte[] {0x9F, 0x1E}),
            new TagTestValue(new byte[] {0x5F, 0x53}),
            new TagTestValue(new byte[] {0x9F, 0x0D}),
            new TagTestValue(new byte[] {0x9F, 0x0E}),
            new TagTestValue(new byte[] {0x9F, 0x0F}),
            new TagTestValue(new byte[] {0x9F, 0x10}),
            new TagTestValue(new byte[] {0x91}),
            new TagTestValue(new byte[] {0x9F, 0x11}),
            new TagTestValue(new byte[] {0x5F, 0x28}),
            new TagTestValue(new byte[] {0x5F, 0x55}),
            new TagTestValue(new byte[] {0x5F, 0x56}),
            new TagTestValue(new byte[] {0x42}),
            new TagTestValue(new byte[] {0x90}),
            new TagTestValue(new byte[] {0x9F, 0x32}),
            new TagTestValue(new byte[] {0x92}),
            new TagTestValue(new byte[] {0x86}),
            new TagTestValue(new byte[] {0x9F, 0x18}),
            new TagTestValue(new byte[] {0x71}),
            new TagTestValue(new byte[] {0x72}),
            new TagTestValue(new byte[] {0x5F, 0x50}),
            new TagTestValue(new byte[] {0x5F, 0x2D}),
            new TagTestValue(new byte[] {0x9F, 0x13}),
            new TagTestValue(new byte[] {0x9F, 0x4D}),
            new TagTestValue(new byte[] {0x9F, 0x4F}),
            new TagTestValue(new byte[] {0x9F, 0x14}),
            new TagTestValue(new byte[] {0x9F, 0x15}),
            new TagTestValue(new byte[] {0x9F, 0x16}),
            new TagTestValue(new byte[] {0x9F, 0x4E}),
            new TagTestValue(new byte[] {0x9F, 0x17}),
            new TagTestValue(new byte[] {0x9F, 0x39}),
            new TagTestValue(new byte[] {0x9F, 0x38}),
            new TagTestValue(new byte[] {0x80}),
            new TagTestValue(new byte[] {0x77}),
            new TagTestValue(new byte[] {0x5F, 0x30}),
            new TagTestValue(new byte[] {0x88}),
            new TagTestValue(new byte[] {0x9F, 0x4B}),
            new TagTestValue(new byte[] {0x93}),
            new TagTestValue(new byte[] {0x9F, 0x4A}),
            new TagTestValue(new byte[] {0x9F, 0x33}),
            new TagTestValue(new byte[] {0x9F, 0x1A}),
            new TagTestValue(new byte[] {0x9F, 0x1B}),
            new TagTestValue(new byte[] {0x9F, 0x1C}),
            new TagTestValue(new byte[] {0x9F, 0x1D}),
            new TagTestValue(new byte[] {0x9F, 0x35}),
            new TagTestValue(new byte[] {0x95}),
            new TagTestValue(new byte[] {0x9F, 0x1F}),
            new TagTestValue(new byte[] {0x9F, 0x20}),
            new TagTestValue(new byte[] {0x57}),
            new TagTestValue(new byte[] {0x98}),
            new TagTestValue(new byte[] {0x97}),
            new TagTestValue(new byte[] {0x5F, 0x2A}),
            new TagTestValue(new byte[] {0x5F, 0x36}),
            new TagTestValue(new byte[] {0x9A}),
            new TagTestValue(new byte[] {0x99}),
            new TagTestValue(new byte[] {0x9F, 0x3C}),
            new TagTestValue(new byte[] {0x9F, 0x3D}),
            new TagTestValue(new byte[] {0x9F, 0x41}),
            new TagTestValue(new byte[] {0x9B}),
            new TagTestValue(new byte[] {0x9F, 0x21}),
            new TagTestValue(new byte[] {0x9C}),
            new TagTestValue(new byte[] {0x9F, 0x37}),
            new TagTestValue(new byte[] {0x9F, 0x23})
        };

        #endregion

        #region Instance Members

        public static IEnumerable<object[]> GetValidTags()
        {
            for (int i = 0; i < ValidTags.Count; i++)
                yield return new object[] {ValidTags[i]};
        }

        #endregion
    }
}