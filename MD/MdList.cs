using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.BD;
using CLGenerator.MD.MdMaterial;
using CLGenerator.ST;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator.MD
{
    public class MdList
    {
        MdBoard _currentBoard { get; set; }
        List<MdBoard> _boards { get; set; } = new List<MdBoard>();

        IPointOrderStrategy _boardEdgeOrdStrgy;
        IDcDimRestriction _restrictions;
        IStBoardOrder _boardOrder;


        public MdList(IPointOrderStrategy boardEdgeOrdStrgy, DcKerfRestriction restrictions, IStBoardOrder boardOrder)
        {
            _restrictions = restrictions;
            _boardEdgeOrdStrgy = boardEdgeOrdStrgy;
            _boardOrder = boardOrder;
        }


        /// <summary>
        /// Get the next available board in list.
        /// </summary>
        /// <returns>The board.</returns>
        public MdBoard CycleBoard(MdDimension dim)
        {
            var index = _boards.IndexOf(_currentBoard);
            if (index + 1 == _boards.Count()){
                _currentBoard = new MdBoard(_boardEdgeOrdStrgy, _restrictions, dim.Material);
                _boards.Add(_currentBoard);
            }
            else{
                _currentBoard = _boards[index + 1];
            }
            return _currentBoard;
        }
       

        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="alignment">Alignment.</param>
        public void AddAlignment(Align alignment){
            _currentBoard.AddAlignment(alignment);
            _resetCurrentBoard();
        }


        public void OrderBoards(){
            _boards = _boardOrder.Implement(_boards);
        }


        public MdBoard GetCurrentBoard(){
            return _currentBoard;
        }


        public Document Write(PdfContentByte cb, Document doc)
        {
            foreach(MdBoard b in _boards){
                doc.NewPage();
                cb.SetColorStroke(new BaseColor(25, 75, 159));
                cb.SetColorFill(new BaseColor(230, 230, 230));
                cb = b.WriteBoard(cb);

                cb.FillStroke();
                cb = b.WritePieces(cb);
                cb = b.WriteCutlines(cb);
            }
            return doc;
        }

        void _resetCurrentBoard(){
            _currentBoard = _boards.First();
        }
    }
}
