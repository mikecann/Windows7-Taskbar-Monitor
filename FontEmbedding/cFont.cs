using System;
using System.Collections.Generic;
using System.Text;

namespace FontEmbedding{

    ///////////////////////////////////////////////////////////
    // (c) 2007 Michael Kaplan
    ///////////////////////////////////////////////////////////{

    using System;
    using System.IO;
    using System.Text;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;

    public class cFont
    {
        // error codes from t2embed.h
        internal enum E : uint
        {
            // Error codes
            NONE = 0x0000,
            API_NOTIMPL = 0x0001,

            // Top level error codes
            CHARCODECOUNTINVALID = 0x0002,
            CHARCODESETINVALID = 0x0003,
            DEVICETRUETYPEFONT = 0x0004,
            HDCINVALID = 0x0006,
            NOFREEMEMORY = 0x0007,
            FONTREFERENCEINVALID = 0x0008,
            NOTATRUETYPEFONT = 0x000A,
            ERRORACCESSINGFONTDATA = 0x000C,
            ERRORACCESSINGFACENAME = 0x000D,
            ERRORUNICODECONVERSION = 0x0011,
            ERRORCONVERTINGCHARS = 0x0012,
            EXCEPTION = 0x0013,
            RESERVEDPARAMNOTNULL = 0x0014,
            CHARSETINVALID = 0x0015,
            FILE_NOT_FOUND = 0x0017,
            TTC_INDEX_OUT_OF_RANGE = 0x0018,
            INPUTPARAMINVALID = 0x0019,

            // Indep level error codes
            ERRORCOMPRESSINGFONTDATA = 0x0100,
            FONTDATAINVALID = 0x0102,
            NAMECHANGEFAILED = 0x0103,
            FONTNOTEMBEDDABLE = 0x0104,
            PRIVSINVALID = 0x0105,
            SUBSETTINGFAILED = 0x0106,
            READFROMSTREAMFAILED = 0x0107,
            SAVETOSTREAMFAILED = 0x0108,
            NOOS2 = 0x0109,
            T2NOFREEMEMORY = 0x010A,
            ERRORREADINGFONTDATA = 0x010B,
            FLAGSINVALID = 0x010C,
            ERRORCREATINGFONTFILE = 0x010D,
            FONTALREADYEXISTS = 0x010E,
            FONTNAMEALREADYEXISTS = 0x010F,
            FONTINSTALLFAILED = 0x0110,
            ERRORDECOMPRESSINGFONTDATA = 0x0111,
            ERRORACCESSINGEXCLUDELIST = 0x0112,
            FACENAMEINVALID = 0x0113,
            STREAMINVALID = 0x0114,
            STATUSINVALID = 0x0115,
            PRIVSTATUSINVALID = 0x0116,
            PERMISSIONSINVALID = 0x0117,
            PBENABLEDINVALID = 0x0118,
            SUBSETTINGEXCEPTION = 0x0119,
            SUBSTRING_TEST_FAIL = 0x011A,
            FONTVARIATIONSIMULATED = 0x011B,
            FONTFAMILYNAMENOTINFULL = 0x011D,

            // Bottom level error codes
            ADDFONTFAILED = 0x0200,
            COULDNTCREATETEMPFILE = 0x0201,
            FONTFILECREATEFAILED = 0x0203,
            WINDOWSAPI = 0x0204,
            FONTFILENOTFOUND = 0x0205,
            RESOURCEFILECREATEFAILED = 0x0206,
            ERROREXPANDINGFONTDATA = 0x0207,
            ERRORGETTINGDC = 0x0208,
            EXCEPTIONINDECOMPRESSION = 0x0209,
            EXCEPTIONINCOMPRESSION = 0x020A,
        }

        // Charset flags for ulCharSet field of TTEmbedFont
        internal enum CHARSET : uint
        {
            UNICODE = 1,
            DEFAULT = 1,
            SYMBOL = 2,
            GLYPHIDX = 3,
        }

        // Status returned by TTLoadEmbeddedFont
        internal enum EMBED : uint
        {
            PREVIEWPRINT = 1,
            EDITABLE = 2,
            INSTALLABLE = 3,
            NOEMBEDDING = 4,
        }

        // Use restriction flags
        internal enum LICENSE : uint
        {
            INSTALLABLE = 0x0000,
            DEFAULT = 0x0000,
            NOEMBEDDING = 0x0002,
            PREVIEWPRINT = 0x0004,
            EDITABLE = 0x0008,
        }

        // Options given to TTEmbedFont in uFlags parameter
        internal enum TTEMBED : uint
        {
            RAW = 0x00000000,
            SUBSET = 0x00000001,
            TTCOMPRESSED = 0x00000004,
            FAILIFVARIATIONSIMULATED = 0x00000010,
            // Embed EUDC font. If there is typeface EUDC embed it otherwise embed system EUDC.
            EMBEDEUDC = 0x00000020,
            WEBOBJECT = 0x00000080,
            XORENCRYPTDATA = 0x10000000,

            // Bits returned through pulStatus for TTEmbedFont
            VARIATIONSIMULATED = 0x00000001,
            // Bit set if EUDC embed success.       
            EUDCEMBEDDED = 0x00000002,
            // Bit set if font embedding permissions indicate no subset and subset requested by client. 
            SUBSETCANCEL = 0x00000004,
        }

        // Flag options for TTLoadEmbeddedFont
        internal enum TTLOAD : uint
        {
            PRIVATE = 0x00000001,
            // If typeface already has EUDC, overwrite setting.
            EUDC_OVERWRITE = 0x00000002,

            // Bits returned through pulStatus for TTLoadEmbeddedFont
            FONT_SUBSETTED = 0x00000001,
            FONT_IN_SYSSTARTUP = 0x00000002,
            EUDC_SET = 0x00000004,
        }

        // Flag options for TTDeleteEmbeddedFont
        internal enum TTDELETE : uint
        {
            DONTREMOVEFONT = 0x00000001,
        }

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal delegate uint WRITEEMBEDPROC(FileStream lpvWriteStream, IntPtr lpvBuffer, uint cbBuffer);

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal delegate uint READEMBEDPROC(FileStream lpvReadStream, IntPtr lpvBuffer, uint cbBuffer);

        [DllImport("T2embed.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern E TTDeleteEmbeddedFont(IntPtr hFontReference,
                                                      TTDELETE ulFlags,
                                                      out uint pulStatus);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal class TTEMBEDINFO
        {
            internal ushort usStructSize;
            internal ushort usRootStrSize;
            internal IntPtr pusRootStr;
        };

        [DllImport("T2embed.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern E TTEmbedFont(IntPtr hDC,
                                             TTEMBED ulFlags,
                                             CHARSET ulCharSet,
                                             out EMBED pulPrivStatus,
                                             out uint pulStatus,
                                             WRITEEMBEDPROC lpfnWriteToStream,
                                             FileStream lpvWriteStream,
                                             IntPtr pusCharCodeSet,
                                             ushort usCharCodeCount,
                                             ushort usLanguage,
                                             TTEMBEDINFO pTTEmbedInfo);

        [DllImport("T2embed.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern E TTEmbedFont(IntPtr hDC,
                                             TTEMBED ulFlags,
                                             CHARSET ulCharSet,
                                             out EMBED pulPrivStatus,
                                             out uint pulStatus,
                                             WRITEEMBEDPROC lpfnWriteToStream,
                                             FileStream lpvWriteStream,
                                             ref ushort pusCharCodeSet,
                                             ushort usCharCodeCount,
                                             ushort usLanguage,
                                             TTEMBEDINFO pTTEmbedInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal class TTLOADINFO
        {
            internal ushort usStructSize;
            internal ushort usRefStrSize;
            internal IntPtr pusRefStr;
        };

        [DllImport("T2embed.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern E TTLoadEmbeddedFont(out IntPtr phFontReference,
                                                    TTLOAD ulFlags,
                                                    out EMBED pulPrivStatus,
                                                    LICENSE ulPrivs,
                                                    out TTLOAD pulStatus,
                                                    READEMBEDPROC lpfnReadFromStream,
                                                    FileStream lpvReadStream,
                                                    [MarshalAs(UnmanagedType.LPWStr)] string szWinFamilyName,
                                                    [MarshalAs(UnmanagedType.LPStr)] string szMacFamilyName,
                                                    TTLOADINFO pTTLoadInfo);

        internal const int LOGPIXELSX = 88;    /* Logical pixels/inch in X */
        internal const int LOGPIXELSY = 90;    /* Logical pixels/inch in Y */
        internal const uint GDI_ERROR = 0xffffffff;
        //internal const string FONTNAME = ".\\FreeBarCode.bin";

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CreateDCW")]
        internal static extern IntPtr CreateDC(string lpszDriver, IntPtr lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern uint GetFontData(IntPtr hdc, uint dwTable, uint dwOffset, IntPtr lpvBuffer, uint cbData);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        internal static extern uint GetFontData(IntPtr hdc, uint dwTable, uint dwOffset, byte[] lpvBuffer, uint cbData);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "CreateFontW")]
        internal static extern IntPtr CreateFont(int nHeight,              // height of font
                                                 int nWidth,               // average character width
                                                 int nEscapement,          // angle of escapement
                                                 int nOrientation,         // base-line orientation angle
                                                 int fnWeight,             // font weight
                                                 uint fdwItalic,           // italic attribute option
                                                 uint fdwUnderline,        // underline attribute option
                                                 uint fdwStrikeOut,        // strikeout attribute option
                                                 uint fdwCharSet,          // character set identifier
                                                 uint fdwOutputPrecision,  // output precision
                                                 uint fdwClipPrecision,    // clipping precision
                                                 uint fdwQuality,          // output quality
                                                 uint fdwPitchAndFamily,   // pitch and family
                                                 string lpszFace);         // typeface name

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);

        private IntPtr m_hFontReference = IntPtr.Zero;
        private IntPtr m_hFontEmbedded = IntPtr.Zero;
        private IntPtr m_hFontOld = IntPtr.Zero;
        private PrivateFontCollection m_pfc = null;

        internal uint WriteEmbedProc(FileStream lpvWriteStream, IntPtr lpvBuffer, uint cbBuffer)
        {
            byte[] rgbyt = new byte[cbBuffer];
            Marshal.Copy(lpvBuffer, rgbyt, 0, (int)cbBuffer);
            lpvWriteStream.Write(rgbyt, 0, (int)cbBuffer);
            return cbBuffer;
        }

        internal uint ReadEmbedProc(FileStream lpvReadStream, IntPtr lpvBuffer, uint cbBuffer)
        {
            byte[] rgbyt = new byte[cbBuffer];
            lpvReadStream.Read(rgbyt, 0, (int)cbBuffer);
            Marshal.Copy(rgbyt, 0, lpvBuffer, (int)cbBuffer);
            return cbBuffer;
        }

        public void CreateFontFile(string pFontName,string pFileName)
        {
                E rc = 0;
                EMBED ulPrivStatus;
                float siz = 32f;

                if (!File.Exists(pFileName))
                {
                    // Create the font and put it where we can use it
                    IntPtr hDC = CreateDC("DISPLAY", IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                    if (hDC != IntPtr.Zero)
                    {
                        IntPtr hFont = CreateFont(MulDiv(Convert.ToInt16(siz), GetDeviceCaps(hDC, LOGPIXELSY), 72),
                                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, pFontName);
                        if (hFont != IntPtr.Zero)
                        {
                            IntPtr hFontOld = SelectObject(hDC, hFont);
                            if (hFontOld != IntPtr.Zero)
                            {
                                // We are writing out the embed file info for the font if the file doesn't exist.
                                uint ulStatus = 0;
                                FileStream fsWrite = new FileStream(pFileName, FileMode.CreateNew);
                                WRITEEMBEDPROC wep = new WRITEEMBEDPROC(this.WriteEmbedProc);
                                TTEMBEDINFO ttie = new TTEMBEDINFO();

                                ttie.usStructSize = Convert.ToUInt16(Marshal.SizeOf(ttie));
                                ttie.usRootStrSize = 0;
                                ttie.pusRootStr = IntPtr.Zero;
                                ulPrivStatus = 0;
                                ulStatus = 0;
                                rc = TTEmbedFont(hDC,
                                                 TTEMBED.RAW | TTEMBED.TTCOMPRESSED,
                                                 CHARSET.UNICODE,
                                                 out ulPrivStatus,
                                                 out ulStatus,
                                                 wep,
                                                 fsWrite,
                                                 IntPtr.Zero,
                                                 0,
                                                 0,
                                                 ttie);
                                fsWrite.Flush();
                                fsWrite.Close();
                                if (rc != E.NONE)
                                {
                                    // Since creation of the file ultimately failed, delete whatever 
                                    // interim bits might have been written.
                                    File.Delete(pFileName);
                                }
                                SelectObject(hDC, hFontOld);
                            }
                            DeleteObject(hFont);
                        }
                        DeleteDC(hDC);
                    }
                }
        }

        public Font CreateFontFromFile(System.IntPtr pHandle, string pFontName, string pFileName)
        {
                E rc = 0;
                EMBED ulPrivStatus;
                float siz = 32f;

            if (File.Exists(pFileName))
            {
                // We are reading in the embed file info if the file exists (we may have just created it!)
                TTLOAD ulStatusRead = 0;
                FileStream fsRead = new FileStream(pFileName, FileMode.Open);
                READEMBEDPROC rep = new READEMBEDPROC(this.ReadEmbedProc);
                TTLOADINFO ttli = new TTLOADINFO();

                ttli.usStructSize = Convert.ToUInt16(Marshal.SizeOf(ttli));
                ttli.usRefStrSize = 0;
                ttli.pusRefStr = IntPtr.Zero;
                ulPrivStatus = 0;

                FileInfo fi = new FileInfo(pFileName);
                rc = TTLoadEmbeddedFont(out this.m_hFontReference, TTLOAD.PRIVATE,
                                        out ulPrivStatus,
                                        LICENSE.EDITABLE, out ulStatusRead,
                                        rep, fsRead,
                                        pFontName, pFontName,
                                        ttli);
                fsRead.Flush();
                fsRead.Close();

                IntPtr hdc = GetDC(pHandle);

                // One way or the other, we ought to have a 'NyalaSIAO' available to us, so let's use it
                this.m_hFontEmbedded = CreateFont(MulDiv(Convert.ToInt16(siz), GetDeviceCaps(hdc, LOGPIXELSY), 72),
                                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, pFontName);
                if (this.m_hFontEmbedded != IntPtr.Zero)
                {
                    this.m_hFontOld = SelectObject(hdc, this.m_hFontEmbedded);
                    uint cb = GetFontData(hdc, 0, 0, IntPtr.Zero, 0);
                    if (cb == GDI_ERROR)
                    {
                        return new Font(pFontName, siz);
                    }
                    else
                    {
                        byte[] rgbyt = new byte[cb];
                        GetFontData(hdc, 0, 0, rgbyt, cb);
                        this.m_pfc = new PrivateFontCollection();
                        IntPtr pbyt = Marshal.AllocCoTaskMem(rgbyt.Length);
                        Marshal.Copy(rgbyt, 0, pbyt, rgbyt.Length);
                        this.m_pfc.AddMemoryFont(pbyt, rgbyt.Length);
                        Marshal.FreeCoTaskMem(pbyt);
                        return new Font(this.m_pfc.Families[0], siz);
                    }
                }
                else
                {
                    return new Font(pFontName, siz);
                }
                //if (this.textBox1.Font.Name != pFontName)
                //{
                //    // We had everything but embedding failed anyway.
                //    this.lbl1.Text = "Embedding failed, font is: " + this.textBox1.Font.Name;
                //}
            }
            else
            {
                return null;
            }

            //// The string is "Amharic (Ethiopia)" in Amharic
            //this.textBox1.Text = "12345";
            //this.textBox1.SelectionStart = this.textBox1.Text.Length;

            //// Control #2 will always try to use Nyala.
            //this.textBox2.Font = new Font("Free 3 of 9 Extended", siz);
            //this.textBox2.Text = this.textBox1.Text;
            //this.textBox2.SelectionStart = this.textBox1.SelectionStart;

        }

        //public Form1()
        //{
        //    InitializeComponent();
        //    E rc = 0;
        //    EMBED ulPrivStatus;
        //    float siz = 32f;

        //    if (Environment.OSVersion.Version >= new Version(6, 0) &&
        //        !File.Exists(FONTNAME))
        //    {
        //        // Create the font and put it where we can use it
        //        IntPtr hDC = CreateDC("DISPLAY", IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        //        if (hDC != IntPtr.Zero)
        //        {
        //            IntPtr hFont = CreateFont(MulDiv(Convert.ToInt16(siz), GetDeviceCaps(hDC, LOGPIXELSY), 72),
        //                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, "Free 3 of 9 Extended");
        //            if (hFont != IntPtr.Zero)
        //            {
        //                IntPtr hFontOld = SelectObject(hDC, hFont);
        //                if (hFontOld != IntPtr.Zero)
        //                {
        //                    // We are writing out the embed file info for the font if the file doesn't exist.
        //                    uint ulStatus = 0;
        //                    FileStream fsWrite = new FileStream(FONTNAME, FileMode.CreateNew);
        //                    WRITEEMBEDPROC wep = new WRITEEMBEDPROC(this.WriteEmbedProc);
        //                    TTEMBEDINFO ttie = new TTEMBEDINFO();

        //                    ttie.usStructSize = Convert.ToUInt16(Marshal.SizeOf(ttie));
        //                    ttie.usRootStrSize = 0;
        //                    ttie.pusRootStr = IntPtr.Zero;
        //                    ulPrivStatus = 0;
        //                    ulStatus = 0;
        //                    rc = TTEmbedFont(hDC,
        //                                     TTEMBED.RAW | TTEMBED.TTCOMPRESSED,
        //                                     CHARSET.UNICODE,
        //                                     out ulPrivStatus,
        //                                     out ulStatus,
        //                                     wep,
        //                                     fsWrite,
        //                                     IntPtr.Zero,
        //                                     0,
        //                                     0,
        //                                     ttie);
        //                    fsWrite.Flush();
        //                    fsWrite.Close();
        //                    if (rc != E.NONE)
        //                    {
        //                        // Since creation of the file ultimately failed, delete whatever 
        //                        // interim bits might have been written.
        //                        File.Delete(FONTNAME);
        //                    }
        //                    SelectObject(hDC, hFontOld);
        //                }
        //                DeleteObject(hFont);
        //            }
        //            DeleteDC(hDC);
        //        }
        //    }

        //    if (File.Exists(FONTNAME))
        //    {
        //        // We are reading in the embed file info if the file exists (we may have just created it!)
        //        TTLOAD ulStatusRead = 0;
        //        FileStream fsRead = new FileStream(FONTNAME, FileMode.Open);
        //        READEMBEDPROC rep = new READEMBEDPROC(this.ReadEmbedProc);
        //        TTLOADINFO ttli = new TTLOADINFO();

        //        ttli.usStructSize = Convert.ToUInt16(Marshal.SizeOf(ttli));
        //        ttli.usRefStrSize = 0;
        //        ttli.pusRefStr = IntPtr.Zero;
        //        ulPrivStatus = 0;

        //        FileInfo fi = new FileInfo(FONTNAME);
        //        rc = TTLoadEmbeddedFont(out this.m_hFontReference, TTLOAD.PRIVATE,
        //                                out ulPrivStatus,
        //                                LICENSE.EDITABLE, out ulStatusRead,
        //                                rep, fsRead,
        //                                "Free 3 of 9 Extended", "Free 3 of 9 Extended",
        //                                ttli);
        //        fsRead.Flush();
        //        fsRead.Close();

        //        IntPtr hdc = GetDC(this.textBox1.Handle);

        //        // One way or the other, we ought to have a 'NyalaSIAO' available to us, so let's use it
        //        this.m_hFontEmbedded = CreateFont(MulDiv(Convert.ToInt16(siz), GetDeviceCaps(hdc, LOGPIXELSY), 72),
        //                                          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, "Free 3 of 9 Extended");
        //        if (this.m_hFontEmbedded != IntPtr.Zero)
        //        {
        //            this.m_hFontOld = SelectObject(hdc, this.m_hFontEmbedded);
        //            uint cb = GetFontData(hdc, 0, 0, IntPtr.Zero, 0);
        //            if (cb == GDI_ERROR)
        //            {
        //                this.textBox1.Font = new Font("Free 3 of 9 Extended", siz);
        //            }
        //            else
        //            {
        //                byte[] rgbyt = new byte[cb];
        //                GetFontData(hdc, 0, 0, rgbyt, cb);
        //                this.m_pfc = new PrivateFontCollection();
        //                IntPtr pbyt = Marshal.AllocCoTaskMem(rgbyt.Length);
        //                Marshal.Copy(rgbyt, 0, pbyt, rgbyt.Length);
        //                this.m_pfc.AddMemoryFont(pbyt, rgbyt.Length);
        //                Marshal.FreeCoTaskMem(pbyt);
        //                this.textBox1.Font = new Font(this.m_pfc.Families[0], siz);
        //            }
        //        }
        //        else
        //        {
        //            // No NyalaSAIO, try and fall back to Nyala
        //            this.textBox1.Font = new Font("Free 3 of 9 Extended", siz);
        //            this.lbl1.Text = "Embedding failed, font is: " + this.textBox1.Font.Name;
        //        }
        //        if (this.textBox1.Font.Name != "Free 3 of 9 Extended")
        //        {
        //            // We had everything but embedding failed anyway.
        //            this.lbl1.Text = "Embedding failed, font is: " + this.textBox1.Font.Name;
        //        }
        //    }

        //    // The string is "Amharic (Ethiopia)" in Amharic
        //    this.textBox1.Text = "12345";
        //    this.textBox1.SelectionStart = this.textBox1.Text.Length;

        //    // Control #2 will always try to use Nyala.
        //    this.textBox2.Font = new Font("Free 3 of 9 Extended", siz);
        //    this.textBox2.Text = this.textBox1.Text;
        //    this.textBox2.SelectionStart = this.textBox1.SelectionStart;
        //}

        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    // Cleanup stuff now
        //    if (this.m_pfc != null)
        //    {
        //        this.m_pfc.Dispose();
        //    }
        //    if (this.m_hFontOld != IntPtr.Zero)
        //    {
        //        SelectObject(GetDC(this.textBox1.Handle), this.m_hFontOld);
        //    }
        //    if (this.m_hFontEmbedded != IntPtr.Zero)
        //    {
        //        DeleteObject(this.m_hFontEmbedded);
        //    }
        //    if (this.m_hFontReference != IntPtr.Zero)
        //    {
        //        uint ulStatus;
        //        TTDeleteEmbeddedFont(this.m_hFontReference, 0, out ulStatus);
        //        if (File.Exists(FONTNAME))
        //        {
        //            // *******************************************************
        //            // WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
        //            // WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
        //            // WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
        //            // *******************************************************
        //            // Nyala has Editable embedding only!
        //            // So usually you would have to run the line of code below!
        //            // File.Delete(FONTNAME);
        //            // *******************************************************
        //            // WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
        //            // WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
        //            // WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
        //            // *******************************************************
        //        }
        //    }
        //}


    }
}