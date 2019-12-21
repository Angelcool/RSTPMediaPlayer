using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace videoPlayer.Util
{
    class LayoutUtils
    {

        public static LayoutUtils layoutUtils = new LayoutUtils();

        private int _xAmount = 1;       //x轴数量
        private int _yAmount = 1;       //y轴数量
        private int _xInterval = 0;     //横向间隔
        private int _yInterval = 0;     //纵向间隔

        /// <summary>
        /// 窗口模式
        /// </summary>
        private SchemasEnums _currentSchama = SchemasEnums.WD1;

        private int _initLocationX = 0;  //初始位置X轴
        private int _initLocationY = 0;  //初始位置Y轴
        private int _interval = 2; //间隔大小

        public LayoutUtils() { }

        public static LayoutUtils INSTANCE()
        {
            return layoutUtils;
        }

        public int getXAmount()
        {
            return _xAmount;
        }

        public int getYAmount()
        {
            return _yAmount;
        }

        public int getXInterval()
        {
            return _xInterval;
        }

        public int getYInterval()
        {
            return _yInterval;
        }

        public SchemasEnums getSchemasEnums() {
            return _currentSchama;
        }

        public int getLocationX()
        {
            return _initLocationX;
        }

        public int getLocationY()
        {
            return _initLocationY;
        }

        /// <summary>
        /// 修改模板变量
        /// </summary>
        /// <param name="videoPanelCount"></param>
        public void definedSchemaParameter(int videoPanelCount)
        {
            SchemasEnums tempSchema = SchemasEnums.WD1;
            if (videoPanelCount == 1)
            {
                _xAmount = 1;       //x轴数量
                _yAmount = 1;       //y轴数量
                _xInterval = 0;     //横向间隔
                _yInterval = 0;     //纵向间隔

            }
            if (videoPanelCount > 1 && videoPanelCount <= 4)
            {
                _xAmount = 2;       //x轴数量
                _yAmount = 2;       //y轴数量
                _xInterval = 4;     //横向间隔
                _yInterval = 4;     //纵向间隔
                tempSchema = SchemasEnums.WD4;
            }
            if (videoPanelCount > 4 && videoPanelCount <= 6)
            {
                _xAmount = 3;       //x轴数量
                _yAmount = 2;       //y轴数量
                _xInterval = 6;     //横向间隔
                _yInterval = 4;     //纵向间隔
                tempSchema = SchemasEnums.WD6;
            }
            if (videoPanelCount > 6 && videoPanelCount <= 9)
            {
                _xAmount = 3;       //x轴数量
                _yAmount = 3;       //y轴数量
                _xInterval = 6;     //横向间隔
                _yInterval = 6;     //纵向间隔
                tempSchema = SchemasEnums.WD9;
            }
            if (videoPanelCount > 9 && videoPanelCount <= 12)
            {
                _xAmount = 4;       //x轴数量
                _yAmount = 3;       //y轴数量
                _xInterval = 8;     //横向间隔
                _yInterval = 6;     //纵向间隔
                tempSchema = SchemasEnums.WD12;
            }
            if (videoPanelCount > 12 && videoPanelCount <= 16)
            {
                _xAmount = 4;       //x轴数量
                _yAmount = 4;       //y轴数量
                _xInterval = 8;     //横向间隔
                _yInterval = 8;     //纵向间隔
                tempSchema = SchemasEnums.WD16;
            }
            if (videoPanelCount > 16 && videoPanelCount <= 20)
            {
                _xAmount = 5;       //x轴数量
                _yAmount = 4;       //y轴数量
                _xInterval = 10;     //横向间隔
                _yInterval = 8;     //纵向间隔
                tempSchema = SchemasEnums.WD20;
            }
            if (videoPanelCount > 20 && videoPanelCount <= 25)
            {
                _xAmount = 5;       //x轴数量
                _yAmount = 5;       //y轴数量
                _xInterval = 10;     //横向间隔
                _yInterval = 10;     //纵向间隔
                tempSchema = SchemasEnums.WD25;
            }
            if (videoPanelCount > 25 && videoPanelCount <= 30)
            {
                _xAmount = 6;       //x轴数量
                _yAmount = 5;       //y轴数量
                _xInterval = 12;     //横向间隔
                _yInterval = 10;     //纵向间隔
                tempSchema = SchemasEnums.WD30;
            }
            if (tempSchema != _currentSchama)
            {
                _currentSchama = tempSchema;
            }
        }


        /// <summary>
        /// 关闭时使用
        /// </summary>
        /// <param name="videoPanelCount"></param>
        public void definedSchemaParameter2(int videoPanelCount)
        {
            SchemasEnums tempSchema = SchemasEnums.WD1;
            if (videoPanelCount == 1)
            {
                _xAmount = 1;       //x轴数量
                _yAmount = 1;       //y轴数量
                _xInterval = 0;     //横向间隔
                _yInterval = 0;     //纵向间隔

            }
            if (videoPanelCount > 1 && videoPanelCount <= 4)
            {
                _xAmount = 2;       //x轴数量
                _yAmount = 2;       //y轴数量
                _xInterval = 4;     //横向间隔
                _yInterval = 4;     //纵向间隔
                tempSchema = SchemasEnums.WD4;
            }
            if (videoPanelCount > 4 && videoPanelCount <= 6)
            {
                _xAmount = 3;       //x轴数量
                _yAmount = 2;       //y轴数量
                _xInterval = 6;     //横向间隔
                _yInterval = 4;     //纵向间隔
                tempSchema = SchemasEnums.WD6;
            }
            if (videoPanelCount > 6 && videoPanelCount <= 9)
            {
                _xAmount = 3;       //x轴数量
                _yAmount = 3;       //y轴数量
                _xInterval = 6;     //横向间隔
                _yInterval = 6;     //纵向间隔
                tempSchema = SchemasEnums.WD9;
            }
            if (videoPanelCount > 9 && videoPanelCount <= 12)
            {
                _xAmount = 4;       //x轴数量
                _yAmount = 3;       //y轴数量
                _xInterval = 8;     //横向间隔
                _yInterval = 6;     //纵向间隔
                tempSchema = SchemasEnums.WD12;
            }
            if (videoPanelCount > 12 && videoPanelCount <= 16)
            {
                _xAmount = 4;       //x轴数量
                _yAmount = 4;       //y轴数量
                _xInterval = 8;     //横向间隔
                _yInterval = 8;     //纵向间隔
                tempSchema = SchemasEnums.WD16;
            }
            if (videoPanelCount > 16 && videoPanelCount <= 20)
            {
                _xAmount = 5;       //x轴数量
                _yAmount = 4;       //y轴数量
                _xInterval = 10;     //横向间隔
                _yInterval = 8;     //纵向间隔
                tempSchema = SchemasEnums.WD20;
            }
            if (videoPanelCount > 20 && videoPanelCount <= 25)
            {
                _xAmount = 5;       //x轴数量
                _yAmount = 5;       //y轴数量
                _xInterval = 10;     //横向间隔
                _yInterval = 10;     //纵向间隔
                tempSchema = SchemasEnums.WD25;
            }
            if (videoPanelCount > 25 && videoPanelCount <= 30)
            {
                _xAmount = 6;       //x轴数量
                _yAmount = 5;       //y轴数量
                _xInterval = 12;     //横向间隔
                _yInterval = 10;     //纵向间隔
                tempSchema = SchemasEnums.WD30;
            }
            if (tempSchema != _currentSchama)
            {
                _currentSchama = tempSchema;
            }
        }

        /// <summary>
        /// 初始化位置
        /// </summary>
        /// <param name="videoPanelIndex"></param>
        /// <param name="videopaneldisplay"></param>
        public void initLocation(int videoPanelIndex, bool videopaneldisplay, int videopanelwidth, int videopanelHeight)
        {
            if (videopaneldisplay) videopanelwidth = videopanelwidth - 180;

            SchemasEnums tempSchema = LayoutUtils.INSTANCE().getSchemasEnums();
            LayoutUtils layoutUtils = LayoutUtils.INSTANCE();
            var newWidth = (videopanelwidth - _xInterval) / _xAmount;
            var newHeight = (videopanelHeight - _yInterval) / _yAmount;

            if (tempSchema == SchemasEnums.WD1)
            {
                _initLocationX = 0;
                _initLocationY = 0;
                return;
            }
            var schemaValueY = 0; //列数
            if (tempSchema == SchemasEnums.WD4)
            {
                schemaValueY = 2;
            }
            if (tempSchema == SchemasEnums.WD6)
            {
                schemaValueY = 3;
            }
            if (tempSchema == SchemasEnums.WD9)
            {
                schemaValueY = 3;
            }
            if (tempSchema == SchemasEnums.WD12)
            {
                schemaValueY = 4;
            }
            if (tempSchema == SchemasEnums.WD16)
            {
                schemaValueY = 4;
            }
            if (tempSchema == SchemasEnums.WD20)
            {
                schemaValueY = 5;
            }
            if (tempSchema == SchemasEnums.WD25)
            {
                schemaValueY = 5;
            }
            if (tempSchema == SchemasEnums.WD30)
            {
                schemaValueY = 6;
            }
            //第几个
            var a = videoPanelIndex / schemaValueY; //取模  
            var b = videoPanelIndex % schemaValueY; //取余
            if (a < 1 && b == 1)     //第一个
            {
                _initLocationX = 0;
                _initLocationY = 0;
                return;
            }
            if (b == 0)             //整排 整列
            {
                _initLocationX = ((schemaValueY - 1) * newWidth) + (schemaValueY * _interval - _interval);
                _initLocationY = ((a - 1) * newHeight) + ((a - 1) * _interval);
                return;
            }
            if (b > 0)     //非整排 非整列  且不为第一个
            {
                _initLocationX = (b - 1) * newWidth + b * _interval - _interval;
                _initLocationY = a * newHeight + a * _interval;
            }

        }
    }
}
