using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.BD;
using CLGenerator.ST;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator.MD
{
    public class MdList
    {
        MdBoard _currentBoard { get; set; }
        List<MdBoard> _boards { get; set; } = new List<MdBoard>();

        MdDimension _boardTemplate;
        IPointOrderStrategy _boardEdgeOrdStrgy { get; set; }
        IDcDimRestriction _restrictions { get; set; } 


        public MdList(MdDimension boardTemplate, IPointOrderStrategy boardEdgeOrdStrgy, DcKerfRestriction restrictions)
        {
            _restrictions = restrictions;
            _boardTemplate = boardTemplate;
            _boardEdgeOrdStrgy = boardEdgeOrdStrgy;
            _boards.Add(new MdBoard(_boardTemplate, _boardEdgeOrdStrgy, restrictions));
            _resetCurrentBoard();
        }


        public MdBoard CycleBoard()
        {
            var index = _boards.IndexOf(_currentBoard);
            if (index + 1 == _boards.Count()){
                _currentBoard = new MdBoard(_boardTemplate, _boardEdgeOrdStrgy, _restrictions);
                _boards.Add(_currentBoard);
            }
            else{
                _currentBoard = _boards[index + 1];
            }
            return _currentBoard;
        }
       

        public void AddAlignment(Align alignment){
            _currentBoard.AddAlignment(alignment);
            _resetCurrentBoard();
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
                cb = new MdPiece(new MdPoint(0, 0), new MdDimension(96, 48)).Write(cb);
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
