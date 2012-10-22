using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkeletalTracking.Utility
{
    public enum ReadingXmlState
    {
        x,
        y,
        z,
        Joint,
        RecordedSampleID,
        Skeleton
    }
}
