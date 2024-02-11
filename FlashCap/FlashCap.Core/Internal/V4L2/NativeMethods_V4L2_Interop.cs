// This is auto generated code by FlashCap.V4L2Generator [1.8.0]. Do not edit.
// Linux version 4.19.0-19-loongson-3 (abuild@10.40.52.160) (gcc version 8.3.0 (Loongnix 8.3.0-6.lnd.vec.36)) #1 SMP 4.19.190.8.14 Thu Aug 24 08:54:20 UTC 2023
// Thu, 14 Dec 2023 01:30:19 GMT

using System;

namespace FlashCap.Internal.V4L2
{
    internal abstract partial class NativeMethods_V4L2_Interop
    {
        // Common
        public abstract string Label { get; }
        public abstract string Architecture { get; }
        public abstract int sizeof_size_t { get; }
        public abstract int sizeof_off_t { get; }

        // Definitions
        public virtual uint V4L2_CAP_VIDEO_CAPTURE => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_ABGR32 => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_ARGB32 => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_JPEG => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_MJPEG => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_RGB24 => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_RGB332 => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_RGB565 => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_RGB565X => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_UYVY => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_XRGB32 => throw new NotImplementedException();
        public virtual uint V4L2_PIX_FMT_YUYV => throw new NotImplementedException();
        public virtual uint VIDIOC_DQBUF => throw new NotImplementedException();
        public virtual uint VIDIOC_ENUM_FMT => throw new NotImplementedException();
        public virtual uint VIDIOC_ENUM_FRAMEINTERVALS => throw new NotImplementedException();
        public virtual uint VIDIOC_ENUM_FRAMESIZES => throw new NotImplementedException();
        public virtual uint VIDIOC_QBUF => throw new NotImplementedException();
        public virtual uint VIDIOC_QUERYBUF => throw new NotImplementedException();
        public virtual uint VIDIOC_QUERYCAP => throw new NotImplementedException();
        public virtual uint VIDIOC_REQBUFS => throw new NotImplementedException();
        public virtual uint VIDIOC_S_FMT => throw new NotImplementedException();
        public virtual uint VIDIOC_STREAMOFF => throw new NotImplementedException();
        public virtual uint VIDIOC_STREAMON => throw new NotImplementedException();

        // Enums
        public enum V4l2_buf_type
        {
            VIDEO_CAPTURE = 1,
            VIDEO_OUTPUT = 2,
            VIDEO_OVERLAY = 3,
            VBI_CAPTURE = 4,
            VBI_OUTPUT = 5,
            SLICED_VBI_CAPTURE = 6,
            SLICED_VBI_OUTPUT = 7,
            VIDEO_OUTPUT_OVERLAY = 8,
            VIDEO_CAPTURE_MPLANE = 9,
            VIDEO_OUTPUT_MPLANE = 10,
            SDR_CAPTURE = 11,
            SDR_OUTPUT = 12,
            META_CAPTURE = 13,
            PRIVATE = 128,
        }

        public enum v4l2_field
        {
            ANY = 0,
            NONE = 1,
            TOP = 2,
            BOTTOM = 3,
            INTERLACED = 4,
            SEQ_TB = 5,
            SEQ_BT = 6,
            ALTERNATE = 7,
            INTERLACED_TB = 8,
            INTERLACED_BT = 9,
        }

        public enum V4l2_frmivaltypes
        {
            DISCRETE = 1,
            CONTINUOUS = 2,
            STEPWISE = 3,
        }

        public enum V4l2_frmsizetypes
        {
            DISCRETE = 1,
            CONTINUOUS = 2,
            STEPWISE = 3,
        }

        public enum V4l2_memory
        {
            MMAP = 1,
            USERPTR = 2,
            OVERLAY = 3,
            DMABUF = 4,
        }


        // Structures
        public interface ITimespec
        {
            IntPtr Tv_sec
            {
                get;
                set;
            }

            IntPtr Tv_nsec
            {
                get;
                set;
            }

        }
        public virtual ITimespec Create_timespec() => throw new NotImplementedException();

        public interface ITimeval
        {
            IntPtr Tv_sec
            {
                get;
                set;
            }

            IntPtr Tv_usec
            {
                get;
                set;
            }

        }
        public virtual ITimeval Create_timeval() => throw new NotImplementedException();

        public interface IV4l2_buffer
        {
            uint Index
            {
                get;
                set;
            }

            uint Type
            {
                get;
                set;
            }

            uint Bytesused
            {
                get;
                set;
            }

            uint Flags
            {
                get;
                set;
            }

            uint Field
            {
                get;
                set;
            }

            ITimeval Timestamp
            {
                get;
                set;
            }

            IV4l2_timecode Timecode
            {
                get;
                set;
            }

            uint Sequence
            {
                get;
                set;
            }

            uint memory
            {
                get;
                set;
            }

            uint m_offset
            {
                get;
                set;
            }

            UIntPtr m_userptr
            {
                get;
                set;
            }

            IntPtr m_planes
            {
                get;
                set;
            }

            int m_fd
            {
                get;
                set;
            }

            uint length
            {
                get;
                set;
            }

            uint reserved2
            {
                get;
                set;
            }

            uint reserved
            {
                get;
                set;
            }

        }
        public virtual IV4l2_buffer Create_v4l2_buffer() => throw new NotImplementedException();

        public interface v4l2_capability
        {
            byte[] driver
            {
                get;
                set;
            }

            byte[] card
            {
                get;
                set;
            }

            byte[] bus_info
            {
                get;
                set;
            }

            uint version
            {
                get;
                set;
            }

            uint capabilities
            {
                get;
                set;
            }

            uint device_caps
            {
                get;
                set;
            }

            uint[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_capability Create_v4l2_capability() => throw new NotImplementedException();

        public interface v4l2_clip
        {
            v4l2_rect c
            {
                get;
                set;
            }

            IntPtr next
            {
                get;
                set;
            }

        }
        public virtual v4l2_clip Create_v4l2_clip() => throw new NotImplementedException();

        public interface v4l2_fmtdesc
        {
            uint index
            {
                get;
                set;
            }

            uint type
            {
                get;
                set;
            }

            uint flags
            {
                get;
                set;
            }

            byte[] description
            {
                get;
                set;
            }

            uint pixelformat
            {
                get;
                set;
            }

            uint[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_fmtdesc Create_v4l2_fmtdesc() => throw new NotImplementedException();

        public interface v4l2_format
        {
            uint type
            {
                get;
                set;
            }

            v4l2_pix_format fmt_pix
            {
                get;
                set;
            }

            v4l2_pix_format_mplane fmt_pix_mp
            {
                get;
                set;
            }

            IV4l2_window fmt_win
            {
                get;
                set;
            }

            IV4l2_vbi_format fmt_vbi
            {
                get;
                set;
            }

            IV4l2_sliced_vbi_format fmt_sliced
            {
                get;
                set;
            }

            v4l2_sdr_format fmt_sdr
            {
                get;
                set;
            }

            v4l2_meta_format fmt_meta
            {
                get;
                set;
            }

            byte[] fmt_raw_data
            {
                get;
                set;
            }

        }
        public virtual v4l2_format Create_v4l2_format() => throw new NotImplementedException();

        public interface v4l2_fract
        {
            uint numerator
            {
                get;
                set;
            }

            uint denominator
            {
                get;
                set;
            }

        }
        public virtual v4l2_fract Create_v4l2_fract() => throw new NotImplementedException();

        public interface v4l2_frmival_stepwise
        {
            v4l2_fract min
            {
                get;
                set;
            }

            v4l2_fract max
            {
                get;
                set;
            }

            v4l2_fract step
            {
                get;
                set;
            }

        }
        public virtual v4l2_frmival_stepwise Create_v4l2_frmival_stepwise() => throw new NotImplementedException();

        public interface v4l2_frmivalenum
        {
            uint index
            {
                get;
                set;
            }

            uint pixel_format
            {
                get;
                set;
            }

            uint width
            {
                get;
                set;
            }

            uint height
            {
                get;
                set;
            }

            uint type
            {
                get;
                set;
            }

            v4l2_fract discrete
            {
                get;
                set;
            }

            v4l2_frmival_stepwise stepwise
            {
                get;
                set;
            }

            uint[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_frmivalenum Create_v4l2_frmivalenum() => throw new NotImplementedException();

        public interface v4l2_frmsize_discrete
        {
            uint width
            {
                get;
                set;
            }

            uint height
            {
                get;
                set;
            }

        }
        public virtual v4l2_frmsize_discrete Create_v4l2_frmsize_discrete() => throw new NotImplementedException();

        public interface v4l2_frmsize_stepwise
        {
            uint min_width
            {
                get;
                set;
            }

            uint max_width
            {
                get;
                set;
            }

            uint step_width
            {
                get;
                set;
            }

            uint min_height
            {
                get;
                set;
            }

            uint max_height
            {
                get;
                set;
            }

            uint step_height
            {
                get;
                set;
            }

        }
        public virtual v4l2_frmsize_stepwise Create_v4l2_frmsize_stepwise() => throw new NotImplementedException();

        public interface v4l2_frmsizeenum
        {
            uint index
            {
                get;
                set;
            }

            uint pixel_format
            {
                get;
                set;
            }

            uint type
            {
                get;
                set;
            }

            v4l2_frmsize_discrete discrete
            {
                get;
                set;
            }

            v4l2_frmsize_stepwise stepwise
            {
                get;
                set;
            }

            uint[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_frmsizeenum Create_v4l2_frmsizeenum() => throw new NotImplementedException();

        public interface v4l2_meta_format
        {
            uint dataformat
            {
                get;
                set;
            }

            uint buffersize
            {
                get;
                set;
            }

        }
        public virtual v4l2_meta_format Create_v4l2_meta_format() => throw new NotImplementedException();

        public interface v4l2_pix_format
        {
            uint width
            {
                get;
                set;
            }

            uint height
            {
                get;
                set;
            }

            uint pixelformat
            {
                get;
                set;
            }

            uint field
            {
                get;
                set;
            }

            uint bytesperline
            {
                get;
                set;
            }

            uint sizeimage
            {
                get;
                set;
            }

            uint colorspace
            {
                get;
                set;
            }

            uint priv
            {
                get;
                set;
            }

            uint flags
            {
                get;
                set;
            }

            uint ycbcr_enc
            {
                get;
                set;
            }

            uint hsv_enc
            {
                get;
                set;
            }

            uint quantization
            {
                get;
                set;
            }

            uint xfer_func
            {
                get;
                set;
            }

        }
        public virtual v4l2_pix_format Create_v4l2_pix_format() => throw new NotImplementedException();

        public interface v4l2_pix_format_mplane
        {
            uint width
            {
                get;
                set;
            }

            uint height
            {
                get;
                set;
            }

            uint pixelformat
            {
                get;
                set;
            }

            uint field
            {
                get;
                set;
            }

            uint colorspace
            {
                get;
                set;
            }

            v4l2_plane_pix_format[] plane_fmt
            {
                get;
                set;
            }

            byte num_planes
            {
                get;
                set;
            }

            byte flags
            {
                get;
                set;
            }

            byte ycbcr_enc
            {
                get;
                set;
            }

            byte hsv_enc
            {
                get;
                set;
            }

            byte quantization
            {
                get;
                set;
            }

            byte xfer_func
            {
                get;
                set;
            }

            byte[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_pix_format_mplane Create_v4l2_pix_format_mplane() => throw new NotImplementedException();

        public interface v4l2_plane
        {
            uint bytesused
            {
                get;
                set;
            }

            uint length
            {
                get;
                set;
            }

            uint m_mem_offset
            {
                get;
                set;
            }

            UIntPtr m_userptr
            {
                get;
                set;
            }

            int m_fd
            {
                get;
                set;
            }

            uint data_offset
            {
                get;
                set;
            }

            uint[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_plane Create_v4l2_plane() => throw new NotImplementedException();

        public interface v4l2_plane_pix_format
        {
            uint sizeimage
            {
                get;
                set;
            }

            uint bytesperline
            {
                get;
                set;
            }

            ushort[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_plane_pix_format Create_v4l2_plane_pix_format() => throw new NotImplementedException();

        public interface v4l2_rect
        {
            int left
            {
                get;
                set;
            }

            int top
            {
                get;
                set;
            }

            uint width
            {
                get;
                set;
            }

            uint height
            {
                get;
                set;
            }

        }
        public virtual v4l2_rect Create_v4l2_rect() => throw new NotImplementedException();

        public interface v4l2_requestbuffers
        {
            uint count
            {
                get;
                set;
            }

            uint type
            {
                get;
                set;
            }

            uint memory
            {
                get;
                set;
            }

            uint[] reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_requestbuffers Create_v4l2_requestbuffers() => throw new NotImplementedException();

        public interface v4l2_sdr_format
        {
            uint pixelformat
            {
                get;
                set;
            }

            uint buffersize
            {
                get;
                set;
            }

            byte[] Reserved
            {
                get;
                set;
            }

        }
        public virtual v4l2_sdr_format Create_v4l2_sdr_format() => throw new NotImplementedException();

        public interface IV4l2_sliced_vbi_format
        {
            ushort Service_set
            {
                get;
                set;
            }

            ushort[][] Service_lines
            {
                get;
                set;
            }

            uint Io_size
            {
                get;
                set;
            }

            uint[] Reserved
            {
                get;
                set;
            }

        }
        public virtual IV4l2_sliced_vbi_format Create_v4l2_sliced_vbi_format() => throw new NotImplementedException();

        public interface IV4l2_timecode
        {
            uint Type
            {
                get;
                set;
            }

            uint Flags
            {
                get;
                set;
            }

            byte Frames
            {
                get;
                set;
            }

            byte Seconds
            {
                get;
                set;
            }

            byte Minutes
            {
                get;
                set;
            }

            byte Hours
            {
                get;
                set;
            }

            byte[] Userbits
            {
                get;
                set;
            }

        }
        public virtual IV4l2_timecode Create_v4l2_timecode() => throw new NotImplementedException();

        public interface IV4l2_vbi_format
        {
            uint Sampling_rate
            {
                get;
                set;
            }

            uint Offset
            {
                get;
                set;
            }

            uint Samples_per_line
            {
                get;
                set;
            }

            uint Sample_format
            {
                get;
                set;
            }

            int[] Start
            {
                get;
                set;
            }

            uint[] Count
            {
                get;
                set;
            }

            uint Flags
            {
                get;
                set;
            }

            uint[] Reserved
            {
                get;
                set;
            }

        }
        public virtual IV4l2_vbi_format Create_v4l2_vbi_format() => throw new NotImplementedException();

        public interface IV4l2_window
        {
            v4l2_rect W
            {
                get;
                set;
            }

            uint Field
            {
                get;
                set;
            }

            uint Chromakey
            {
                get;
                set;
            }

            IntPtr Clips
            {
                get;
                set;
            }

            uint Clipcount
            {
                get;
                set;
            }

            IntPtr Bitmap
            {
                get;
                set;
            }

            byte Global_alpha
            {
                get;
                set;
            }

        }
        public virtual IV4l2_window Create_v4l2_window() => throw new NotImplementedException();


    }
}

