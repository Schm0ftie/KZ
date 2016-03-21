using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace KZLogAnalyzer.Data
{
    public class LogReader
    {

        private Regex RegJumpInfo = new Regex(@"^\[KZ]\s(.*)\sjumped\s([0-9]*(?:\.[0-9]*)?)\sunits\swith\sa\s(\w+)\s\[(\d+)\sStrafes\s\|\s([0-9]*(?:\.[0-9]*)?)\sPre\s\|\s([0-9]*(?:\.[0-9]*)?)\sMax\s\|\sHeight\s([0-9]*(?:\.[0-9]*)?)\s(?:\|\s(\d+)\sBhops\s){0,1}\|\s([0-9]*(?:\.[0-9]*)?)\%\sSync(?:(?:\s\|(?:\sJumpOff\sEdge\s([0-9]*(?:\.[0-9]*)?))){0,1}\s\|\sCrouchJump:\s(yes|no)\s\|\s-Forward:\s(yes|no)(?:\]\s\[([0-9]*(?:\.[0-9]*)?)\sblock){0,1}){0,1}\]$");
        private Regex RegStrafeInfo = new Regex(@"^\s(\d*).\s*([0-9]*(?:\.[0-9]*)?)%\s*([0-9]*(?:\.[0-9]*)?)\s*([0-9]*(?:\.[0-9]*)?)\s*([0-9]*(?:\.[0-9]*)?)\s*([0-9]*(?:\.[0-9]*)?)%$");

        public List<Jump> ReadLog(string file)
        {
            FileInfo oFileInfo = new FileInfo(@file);
            List<Jump> oJumpList = new List<Jump>();
            using (StreamReader reader = new StreamReader(oFileInfo.FullName))
            {
                string line;
                Jump oJump = null;
                Strafe oStrafe = null;
                while ((line = reader.ReadLine()) != null)
                {
                    Match matchJump = RegJumpInfo.Match(line);
                    if (matchJump.Success)
                        oJump = new Jump((JumpType)Enum.Parse(typeof(JumpType), matchJump.Groups[3].Value));
                        oJump.PlayerName = matchJump.Groups[1].Value;
                        oJump.Distance = float.Parse(matchJump.Groups[2].Value, CultureInfo.InvariantCulture);
                        oJump.StrafeCount = Int32.Parse(matchJump.Groups[4].Value, CultureInfo.InvariantCulture);
                        oJump.Pre = float.Parse(matchJump.Groups[5].Value, CultureInfo.InvariantCulture);
                        oJump.Max = float.Parse(matchJump.Groups[6].Value, CultureInfo.InvariantCulture);
                        oJump.Height = float.Parse(matchJump.Groups[7].Value, CultureInfo.InvariantCulture);
                        oJump.Sync = float.Parse(matchJump.Groups[9].Value, CultureInfo.InvariantCulture);
                        if (oJump.JumpType == JumpType.MultiBhop)
                            oJump.Bhops = Int32.Parse(matchJump.Groups[8].Value, CultureInfo.InvariantCulture);
                        if (oJump.JumpType == JumpType.LongJump)
                        {
                            if (matchJump.Groups[10].Value != String.Empty)
                                oJump.JumpOffEdge = float.Parse(matchJump.Groups[10].Value, CultureInfo.InvariantCulture);
                            oJump.CrouchJump = (matchJump.Groups[11].Value.Equals("yes")) ? true : false;
                            oJump.Forward = (matchJump.Groups[12].Value.Equals("yes")) ? true : false;
                            if (matchJump.Groups[13].Value != String.Empty)
                                oJump.Block = float.Parse(matchJump.Groups[13].Value, CultureInfo.InvariantCulture);

                        }
                        oJumpList.Add(oJump);
                    }
                    if (oJump != null)
                    {
                        Match matchStrafe = RegStrafeInfo.Match(line);
                        if (matchStrafe.Success)
                        {
                            oStrafe = new Strafe(
                                Int32.Parse(matchStrafe.Groups[1].Value, CultureInfo.InvariantCulture),
                                float.Parse(matchStrafe.Groups[2].Value, CultureInfo.InvariantCulture),
                                float.Parse(matchStrafe.Groups[3].Value, CultureInfo.InvariantCulture),
                                float.Parse(matchStrafe.Groups[4].Value, CultureInfo.InvariantCulture),
                                float.Parse(matchStrafe.Groups[5].Value, CultureInfo.InvariantCulture),
                                float.Parse(matchStrafe.Groups[6].Value, CultureInfo.InvariantCulture));
                            oJump.Strafes.Add(oStrafe);
                        }
                        else
                        {
                            if (oJump.Strafes.Count > 0)
                            {
                                oStrafe = null;
                                oJump = null;
                            }
                        }

                    }
                }
            
            return oJumpList;
        }
    }
}
