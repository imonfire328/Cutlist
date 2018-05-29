using System;
using CLGenerator.BD;
using CLGenerator.MD;
using CLGenerator.ST;

namespace CLGenerator.FC
{
    public class OperationTypeFactory : IFactory<Operator>
    {
        string _name;
        MdDimList _dimList;
        double _kerf;

        public OperationTypeFactory(string name, MdDimList dimlist, double kerf)
        {
            _name = name;
            _dimList = dimlist;
            _kerf = kerf;
        }

        public Operator GetInstance()
        {
            switch (_name)
            {
                case "CrossCut":
                    return new Operator(
                        new StDimOrder(
                            new DcOrderWithinBoundsX(
                                new OrderAreaDec(
                                    new StOrientToHeight(
                                        _dimList.GetDimensions()
                                        ).Implement()
                                    ),
                                new MdRectangle(48, 96)
                            )
                        ),
                        new MdList(
                            new OrderPointsByX(),
                            new DcKerfRestriction(
                                new DcDimOverhangYRestriction(
                                ),
                                _kerf
                            ),
                            new ThickAscending()
                        ),
                        new StAlignHorizontal()
                    );

                case "RipCut":
                    return new Operator(
                        new StDimOrder(
                            new DcOrderWidth(
                                new DcOrderWithinBoundsY(
                                    new StOrientToHeight(
                                        _dimList.GetDimensions()
                                    ).Implement(),
                                    new MdRectangle(48, 96)
                                )
                            )
                        ),
                        new MdList(
                            new OrderPointsByY(),
                            new DcKerfRestriction(
                                new DcDimOverhangXRestriction(
                                ),
                                _kerf
                            ),
                            new ThickAscending()
                        ),
                        new StAlignVertical()
                    );

                    default: return null;
                }
            }
        }
    }

