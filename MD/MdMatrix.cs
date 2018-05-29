using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CLGenerator.ST;

namespace CLGenerator.MD
{
    /// <summary>
    /// Represents data on a board gathered from its pieces
    /// </summary>
    public class MdMatrix
    {
        List<IMdLine> _lines { get; set; } = new List<IMdLine>();
        List<MdSection> _sections { get; set; } = new List<MdSection>();
        List<MdPoint> _crossSections { get; set; } = new List<MdPoint>();

        public MdMatrix(){

        }

        /// <summary>
        /// Add details from new piece to matrix
        /// </summary>
        /// <param name="piece">Piece.</param>
        public void UpdateMatrix(Align a)
        {
            if(_lines.Count() == 0){
                _lines = a.Alignment.ToLines();
                return;
            }

            var piece = a.Alignment;
            var lines = piece.ToLines();
            var newLines = new List<IMdLine>(_lines);

            // vertical
            foreach(MdVertLine v in lines.OfType<MdVertLine>()){
                newLines = _mergeNewLine(newLines, v, _lines.OfType<MdVertLine>().FirstOrDefault(r => r.End.Equals(v.Start)));
            }

            // horizontal
            foreach(MdHorLine h in lines.OfType<MdHorLine>()){
                newLines = _mergeNewLine(newLines, h, _lines.OfType<MdHorLine>().FirstOrDefault(r => r.End.Equals(h.Start)));
            }
            _lines = newLines;
        }


        /// <summary>
        /// extends existing or adds new line to list
        /// </summary>
        /// <returns>The new line.</returns>
        /// <param name="lines">Lines.</param>
        /// <param name="newLine">New line.</param>
        /// <param name="curLine">Current line.</param>
        List<IMdLine> _mergeNewLine(List<IMdLine> lines, IMdLineDirection newLine, IMdLineDirection syncLine){
            if (syncLine == null){
                lines.Add(newLine);
            }
            else if(syncLine != null){
                lines.Remove(syncLine);
                syncLine.AddComposite(newLine);
                lines.Add(syncLine);
            }
            else if (syncLine.Equals(newLine)){}
            return lines;
        }


        /// <summary>
        /// Returns a list of lines that span entire board
        /// </summary>
        /// <returns>The cut lines.</returns>
        /// <param name="board">Board.</param>
        public List<IMdLine> GetCutLines(MdBoard board)
        {
            var fullLines = new List<IMdLine>();

            // check vertical lines for full cuts
            foreach(MdVertLine vline in _lines.OfType<MdVertLine>()){
                Debug.WriteLine("Vertical Line: " + vline.id);
                var isFull = true;
                foreach(MdHorLine hline in _lines.OfType<MdHorLine>()){
                    if(vline.Intersects(hline)){
                        isFull = false;
                        continue;
                    }
                }

                // add new vertical line with full height of board
                if(isFull){
                    fullLines.Add(
                        new MdVertLine(
                            vline.Start, 
                            board.SetToHeight(
                                vline.End
                            )
                        )
                    );
                }
            }

            // check horizontal lines for full cuts
            foreach(MdHorLine hline in _lines.OfType<MdHorLine>()){
                Debug.WriteLine("Horizontal Line: " + hline.id);
                var isFull = true; 
                foreach(MdVertLine vline in _lines.OfType<MdVertLine>()){
                    if (hline.Intersects(vline)){
                        isFull = false;
                        continue;
                    }
                }
                // add new vertical line with full height of board
                if (isFull){
                    fullLines.Add(
                        new MdHorLine(
                            hline.Start,
                            board.SetToWidth(
                                hline.End
                            )
                        )
                    );
                }
            }
            return fullLines;
        }
    }
}
