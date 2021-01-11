//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ntreev.Library.Psd.Readers.LayerResources
{
    [ResourceID("lrFX")]
    class Reader_lrFX : ResourceReaderBase
    {
        public Reader_lrFX(PsdReader reader, long length)
            : base(reader, length)
        {

        }
        protected override void ReadValue(PsdReader reader, object userData, out IProperties value)
        {
            value = new Properties(7);

            short version = reader.ReadInt16();
            int count = reader.ReadInt16();
            Properties props = new Properties(7);
            for (int i = 0; i < count; i++)
            {
                string _8bim = reader.ReadAscii(4);
                string effectType = reader.ReadAscii(4);
                int size = reader.ReadInt32();
                long p = reader.Position;

                switch (effectType)
                {
                    case "dsdw":
                        {
                            Properties shadowProps = new Properties(12);
                            shadowProps["Size"] = reader.ReadByte();
                            shadowProps["Version"] = reader.ReadByte();
                            shadowProps["BlurValue"] = reader.ReadInt32();
                            shadowProps["Intensity"] = reader.ReadInt32();
                            shadowProps["Angle"] = reader.ReadInt32();
                            shadowProps["Distance"] = reader.ReadInt32();

                            //Color: 2 bytes for space followed by 4 * 2 byte color component
                            shadowProps["ColorSpace"]= reader.ReadInt16();
                            var   colors = new int[4];
                            for (int j = 0; j < colors.Length; j++)
                            {
                                colors[i] =reader.ReadInt16();
                            }
                            shadowProps["Color"] = colors;
                            props["dsdw"] = shadowProps;
//                            ShadowInfo.Parse(reader);
                        }
                        break;
                    case "sofi":
                        {
                            //this.solidFillInfo = SolidFillInfo.Parse(reader);
                        }
                        break;
                    case "oglw":
                    {
                        Properties shadowProps = new Properties(12);
                        shadowProps["Size"] = reader.ReadByte();
                        shadowProps["Version"] = reader.ReadByte();
                        shadowProps["BlurValue"] = reader.ReadInt32();
                        shadowProps["Intensity"] = reader.ReadInt32();
                        //Color: 2 bytes for space followed by 4 * 2 byte color component
                        shadowProps["ColorSpace"] = reader.ReadInt16();
                        var colors = new int[4];
                        for (int j = 0; j < colors.Length; j++)
                        {
                            colors[i] = reader.ReadInt16();
                        }
                        shadowProps["outerGlowColor"] = colors;
                        props[effectType] = shadowProps;
                            break;
                    }
                }

                reader.Position = p + size;
            }

            value = props;
        }
    }
}
