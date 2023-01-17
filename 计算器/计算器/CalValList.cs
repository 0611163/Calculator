////////////////////////////////////////////////
//核心算法文件，请慎重修改
//2012年7月27日
//修复某些错误无法定位的问题，如9++sqrt(-9*9)，sqrt参数不能为负数
//2012年7月27日
//增加单步计算功能
//最后修改日期：2012年7月
////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    /// <summary>
    /// 运算因子(数字、常数及运算符等)列表，用于把表达式字符串转换成运算因子列表
    /// </summary>
    class CalValList
    {
        /// <summary>
        /// 运算因子列表
        /// </summary>
        public List<CalVal> valList = new List<CalVal>();
        /// <summary>
        /// 规范化之后的算式
        /// </summary>
        public string nomalStringExpression;
        /// <summary>
        /// 优先级列表，按优先级从高到低存储
        /// </summary>
        public List<int> levelList = new List<int>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strExpression">字符串表达式</param>
        public CalValList(string strExpression)
        {
            strExpression = strExpression.Replace(" ", "");//删除字符串中的空格

            if (strExpression == "")
            {
                throw (new Exception("表达式为空"));
            }

            //检查字符串表达式的括号是否匹配
            ParenthesisMatch(strExpression);
            //转换成列表
            ToList(valList, strExpression);
            //规范化
            ToNormalList();
            //设置运算因子在运算因子列表的原始位置(规范化后)
            SetValPrimalPos();
            //优先级处理
            LevelProcess();
            //生成优先级列表
            MakeLevelList();
            //获取规范表达式
            nomalStringExpression = BackToStrExp();
        }
        #endregion

        #region 检查括号是否匹配
        /// <summary>
        /// 检查字符串表达式的括号是否匹配
        /// </summary>
        /// <param name="strExpression">字符串表达式</param>
        private void ParenthesisMatch(string strExpression)
        {
            int leftParenthesis = 0;
            int rightParenthesis = 0;

            for (int i = 0; i < strExpression.Length; i++)
            {
                if (strExpression[i] == '(')
                {
                    leftParenthesis++;
                }

                if (strExpression[i] == ')')
                {
                    rightParenthesis++;
                }
            }

            if (leftParenthesis != rightParenthesis)
            {
                throw (new Exception("括号不匹配"));
            }
        }
        #endregion

        #region 优先级处理
        /// <summary>
        /// 优先级处理，提高括号中的算式的运算符的优先级
        /// </summary>
        private void LevelProcess()
        {
            int delLevel = 0;//优先级增量

            for (int i = 0; i < valList.Count; i++)
            {
                if (valList[i].type == ValType.Operator)
                {
                    //如果是左括号
                    if (valList[i].OpeType == OpeType.leftParenthesis)
                    {
                        delLevel += 1000;
                        continue;
                    }

                    //如果是右括号
                    if (valList[i].OpeType == OpeType.rightParenthesis)
                    {
                        delLevel -= 1000;
                        continue;
                    }

                    valList[i].level = valList[i].ope.level + delLevel;
                }
            }
        }
        #endregion

        #region 运算因子列表转换为字符串表达式
        /// <summary>
        /// 运算因子列表转换为字符串表达式
        /// </summary>
        public string BackToStrExp()
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < valList.Count; i++)
            {
                switch (valList[i].type)
                {
                    case ValType.Number:
                        result.Append(valList[i].num.stringValue);
                        break;
                    case ValType.Operator:
                        result.Append(valList[i].ope.tag);
                        break;
                }
            }

            return result.ToString();
        }
        /// <summary>
        /// 运算因子列表转换为字符串表达式
        /// </summary>
        /// <param name="cutPos">截断位置</param>
        public string BackToStrExp(int cutPos, List<CalVal> valList)
        {
            StringBuilder result = new StringBuilder();

            if (cutPos > valList.Count)
            {
                cutPos = valList.Count;
            }

            for (int i = 0; i < cutPos; i++)
            {
                switch (valList[i].type)
                {
                    case ValType.Number:
                        result.Append(valList[i].num.stringValue);
                        break;
                    case ValType.Operator:
                        result.Append(valList[i].ope.tag);
                        break;
                }
            }

            return result.ToString();
        }
        #endregion

        #region 运算因子(数字、常数及运算符)列表合法性检查，并规范化
        /// <summary>
        /// 运算因子(数字、常数及运算符)列表合法性检查，并规范化
        /// </summary>
        private void ToNormalList()
        {
            CalOpeList opeList = new CalOpeList();

            #region 第一次处理
            //第一次处理，如sin2pi，转换成sin(2pi)
            for (int i = 0; i < valList.Count; i++)
            {
                //如果是数字
                if (valList[i].type == ValType.Number)
                {
                    //如果前面是左括号
                    if (i - 1 >= 0
                        && valList[i - 1].OpeType == OpeType.leftParenthesis)
                    {
                        continue;
                    }

                    //如果前面是一个参数的函数
                    if (i - 1 >= 0
                        && valList[i - 1].OpeType == OpeType.right
                        && valList[i - 1].ParameterNumber == 1)
                    {
                        //如果下一个是常数
                        if (i + 1 < valList.Count
                            && valList[i + 1].OpeType == OpeType.noParameter)
                        {
                            Insert(opeList.GetOpe(")"), i + 2);
                            Insert(opeList.GetOpe("("), i);
                            i++;
                        }
                    }
                }
            }
            #endregion

            #region 第二次处理
            //第二次处理，如sin30度，转换为sin(30度)
            for (int i = 0; i < valList.Count - 1; i++)
            {
                //如果是参数在右边的单目运算符
                if (valList[i].OpeType == OpeType.right
                    && valList[i].ParameterNumber == 1)
                {
                    //如果下一个是运算符且是左括号
                    if (valList[i + 1].OpeType == OpeType.leftParenthesis)
                    {
                        continue;
                    }

                    //如果下一个是常数或数字
                    if (valList[i + 1].OpeType == OpeType.noParameter
                        || valList[i + 1].type == ValType.Number)
                    {
                        //如果下下个运算符是参数在左边的运算符
                        if (i + 2 < valList.Count
                            && valList[i + 2].OpeType == OpeType.left)
                        {
                            Insert(opeList.GetOpe(")"), i + 3);
                            Insert(opeList.GetOpe("("), i + 1);
                            i++;
                        }
                        else
                        {
                            Insert(opeList.GetOpe(")"), i + 2);
                            Insert(opeList.GetOpe("("), i + 1);
                            i++;
                        }

                    }
                }
            }
            #endregion

            #region 第三次处理
            //第三次处理，如3+-3转换为3+(-3)
            for (int i = 1; i < valList.Count - 1; i++)
            {
                //如果是'+'或'-'运算符
                if (valList[i].OpeType == OpeType.PlusOrMinus)
                {
                    //如果前一个是运算符且类型如下
                    if (valList[i - 1].OpeType == OpeType.PlusOrMinus
                        || valList[i - 1].OpeType == OpeType.bothSides
                        || valList[i - 1].OpeType == OpeType.right)
                    {
                        //如果下一个是数字或常数
                        if (valList[i + 1].type == ValType.Number
                            || valList[i + 1].OpeType == OpeType.noParameter)
                        {
                            Insert(opeList.GetOpe(")"), i + 2);
                            Insert(opeList.GetOpe("("), i);
                            i++;
                        }
                    }
                }
            }
            #endregion

            #region 第四次处理
            //第四次处理，判断表达式是否合法，并在合适的地方插入乘号
            for (int i = 0; i < valList.Count - 1; i++)
            {
                #region 当前运算因子是数字
                if (valList[i].type == ValType.Number)//数字
                {
                    if (valList[i + 1].type == ValType.Operator)//右边是运算符
                    {
                        switch (valList[i + 1].OpeType)
                        {
                            case OpeType.bothSides://正确
                                break;
                            case OpeType.left://正确
                                break;
                            case OpeType.leftParenthesis:
                                Insert(opeList.GetOpe("*"), i + 1);
                                break;
                            case OpeType.noParameter:
                                Insert(opeList.GetOpe("*"), i + 1);
                                break;
                            case OpeType.PlusOrMinus://正确
                                break;
                            case OpeType.right:
                                Insert(opeList.GetOpe("*"), i + 1);
                                break;
                            case OpeType.rightParenthesis://正确
                                break;
                            case OpeType.Separator://正确
                                break;
                        }
                    }
                    else//右边是数字
                    {
                        Insert(opeList.GetOpe("*"), i + 1);
                    }
                }
                #endregion

                #region 当前运算因子是运算符
                if (valList[i].type == ValType.Operator)//运算符
                {
                    #region 当前运算符右边是运算符
                    if (valList[i + 1].type == ValType.Operator)//右边是运算符
                    {
                        switch (valList[i].OpeType)//左运算符
                        {
                            #region case OpeType.bothSides://左运算符参数信息
                            case OpeType.bothSides://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides:
                                        MakeException(i);
                                        break;
                                    case OpeType.left:
                                        MakeException(i);
                                        break;
                                    case OpeType.leftParenthesis://正确
                                        break;
                                    case OpeType.noParameter://正确
                                        break;
                                    case OpeType.PlusOrMinus:
                                        MakeException(i);
                                        break;
                                    case OpeType.right://正确
                                        break;
                                    case OpeType.rightParenthesis:
                                        MakeException(i);
                                        break;
                                    case OpeType.Separator:
                                        MakeException(i);
                                        break;
                                }
                                break;
                            #endregion

                            #region case OpeType.left://左运算符参数信息
                            case OpeType.left://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides://正确
                                        break;
                                    case OpeType.left://正确
                                        break;
                                    case OpeType.leftParenthesis:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.noParameter:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.PlusOrMinus://正确
                                        break;
                                    case OpeType.right:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.rightParenthesis://正确
                                        break;
                                    case OpeType.Separator: //正确
                                        break;
                                }
                                break;
                            #endregion

                            #region case OpeType.leftParenthesis://左运算符参数信息
                            case OpeType.leftParenthesis://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides:
                                        int pos = i + 1 >= 0 ? i + 1 : i;
                                        MakeException(pos);
                                        break;
                                    case OpeType.left:
                                        pos = i + 1 >= 0 ? i + 1 : i;
                                        MakeException(pos);
                                        break;
                                    case OpeType.leftParenthesis://正确
                                        break;
                                    case OpeType.noParameter://正确
                                        break;
                                    case OpeType.PlusOrMinus://正确
                                        break;
                                    case OpeType.right://正确
                                        break;
                                    case OpeType.rightParenthesis:
                                        pos = i - 1 >= 0 ? i - 1 : i;
                                        MakeException(pos);
                                        break;
                                    case OpeType.Separator:
                                        pos = i - 1 >= 0 ? i - 1 : i;
                                        MakeException(pos);
                                        break;
                                }
                                break;
                            #endregion

                            #region case OpeType.noParameter://左运算符参数信息
                            case OpeType.noParameter://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides://正确
                                        break;
                                    case OpeType.left://正确
                                        break;
                                    case OpeType.leftParenthesis:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.noParameter:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.PlusOrMinus://正确
                                        break;
                                    case OpeType.right:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.rightParenthesis://正确
                                        break;
                                    case OpeType.Separator: //正确
                                        break;
                                }
                                break;
                            #endregion

                            #region case OpeType.PlusOrMinus://左运算符参数信息
                            case OpeType.PlusOrMinus://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides:
                                        MakeException(i);
                                        break;
                                    case OpeType.left:
                                        MakeException(i);
                                        break;
                                    case OpeType.leftParenthesis://正确
                                        break;
                                    case OpeType.noParameter://正确
                                        break;
                                    case OpeType.PlusOrMinus:
                                        MakeException(i);
                                        break;
                                    case OpeType.right://正确
                                        break;
                                    case OpeType.rightParenthesis:
                                        MakeException(i);
                                        break;
                                    case OpeType.Separator:
                                        MakeException(i);
                                        break;
                                }
                                break;
                            #endregion

                            #region case OpeType.right://左运算符参数信息
                            case OpeType.right://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides:
                                        MakeException(i);
                                        break;
                                    case OpeType.left:
                                        MakeException(i);
                                        break;
                                    case OpeType.leftParenthesis://正确
                                        break;
                                    case OpeType.noParameter:
                                        MakeException(i);
                                        break;
                                    case OpeType.PlusOrMinus:
                                        MakeException(i);
                                        break;
                                    case OpeType.right:
                                        MakeException(i);
                                        break;
                                    case OpeType.rightParenthesis:
                                        MakeException(i);
                                        break;
                                    case OpeType.Separator:
                                        MakeException(i);
                                        break;
                                }
                                break;
                            #endregion

                            #region case OpeType.rightParenthesis://左运算符参数信息
                            case OpeType.rightParenthesis://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides://正确
                                        break;
                                    case OpeType.left://正确
                                        break;
                                    case OpeType.leftParenthesis:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.noParameter:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.PlusOrMinus://正确
                                        break;
                                    case OpeType.right:
                                        Insert(opeList.GetOpe("*"), i + 1);
                                        break;
                                    case OpeType.rightParenthesis://正确
                                        break;
                                    case OpeType.Separator://正确
                                        break;
                                }
                                break;
                            #endregion

                            #region case OpeType.Separator://左运算符参数信息
                            case OpeType.Separator://左运算符参数信息
                                switch (valList[i + 1].OpeType)//右运算符
                                {
                                    case OpeType.bothSides:
                                        MakeException(i + 1);
                                        break;
                                    case OpeType.left:
                                        MakeException(i + 1);
                                        break;
                                    case OpeType.leftParenthesis://正确
                                        break;
                                    case OpeType.noParameter://正确
                                        break;
                                    case OpeType.PlusOrMinus://正确
                                        break;
                                    case OpeType.right://正确
                                        break;
                                    case OpeType.rightParenthesis:
                                        MakeException(i);
                                        break;
                                    case OpeType.Separator:
                                        MakeException(i);
                                        break;
                                }
                                break;
                            #endregion

                        }
                    }
                    #endregion

                    #region 当前运算符右边是数字
                    else if (valList[i + 1].type == ValType.Number)//运算符右边是数字
                    {
                        switch (valList[i].OpeType)
                        {
                            case OpeType.bothSides://正确
                                break;
                            case OpeType.left:
                                Insert(opeList.GetOpe("*"), i + 1);
                                break;
                            case OpeType.leftParenthesis://正确
                                break;
                            case OpeType.noParameter:
                                Insert(opeList.GetOpe("*"), i + 1);
                                break;
                            case OpeType.PlusOrMinus://正确
                                break;
                            case OpeType.right:
                                MakeException(i);
                                break;
                            case OpeType.rightParenthesis:
                                Insert(opeList.GetOpe("*"), i + 1);
                                break;
                            case OpeType.Separator: //正确
                                break;
                        }
                    }
                    #endregion

                }
                #endregion

            }
            #endregion

            #region 第五次处理
            //检查表达式的开头和结尾(第四次处理检查不到的地方)、数字两边括号
            //*9不合法 
            //8-不合法 
            //nCr(9,8,7)不合法
            //(9)%处理成9%
            for (int i = 0; i < valList.Count; i++)//逐个扫描
            {
                #region 如果是运算符
                if (valList[i].type == ValType.Operator)//如果是运算符
                {
                    switch (valList[i].OpeType)
                    {
                        case OpeType.bothSides:
                            if (!(i - 1 >= 0 && i + 1 < valList.Count))
                            {
                                MakeException(i);
                            }
                            break;
                        case OpeType.left:
                            if (!(i - 1 >= 0))
                            {
                                MakeException(i);
                            }
                            break;
                        case OpeType.leftParenthesis:
                            if (!(i + 1 < valList.Count))
                            {
                                MakeException(i);
                            }
                            break;
                        case OpeType.noParameter:
                            break;
                        case OpeType.PlusOrMinus:
                            if (!(i + 1 < valList.Count))
                            {
                                MakeException(i);
                            }
                            break;
                        case OpeType.right:
                            if (!(i + 1 < valList.Count))
                            {
                                MakeException(i);
                            }
                            break;
                        case OpeType.rightParenthesis:
                            if (!(i - 1 >= 0))
                            {
                                MakeException(i);
                            }
                            break;
                        case OpeType.Separator:
                            if (!(i - 1 >= 0 && i + 1 < valList.Count))
                            {
                                MakeException(i);
                            }
                            break;
                    }
                }
                #endregion

                #region 如果是数字
                if (valList[i].type == ValType.Number)//如果是数字
                {
                    if (i - 1 >= 0 && i + 1 < valList.Count)
                    {
                        //两边同时为括号
                        if (valList[i - 1].OpeType == OpeType.leftParenthesis
                            && valList[i + 1].OpeType == OpeType.rightParenthesis)
                        {
                            //如sin(9-7),average(9-7)
                            if (!(i - 2 >= 0
                                && valList[i - 2].OpeType == OpeType.right))
                            {
                                ReplaceVal(valList[i].num, 3, i - 1);
                            }
                        }
                    }
                }
                #endregion

            }
            #endregion

        }
        #endregion

        #region 抛出异常
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="pos">异常在运算因子列表中的位置</param>
        private void MakeException(int pos)
        {
            Common.strWrongExp = BackToStrExp();
            throw (new Exception("在'" + valList[pos].ope.tag + "'附近可能存在错误"
                + "|:" + valList[pos].ope.tag
                + "|:" + BackToStrExp(pos, valList).Length.ToString()));
        }
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="pos">异常在运算因子列表中的位置</param>
        private void MakeException(int pos, Exception ex)
        {
            List<CalVal> tempValList = new CalValList(nomalStringExpression).valList;
            throw (new Exception(ex.Message
                + "|:" + tempValList[pos].ope.tag
                + "|:" + BackToStrExp(pos, tempValList).Length.ToString()));
        }
        #endregion

        #region 插入一个运算因子
        /// <summary>
        /// 插入一个运算因子
        /// </summary>
        private void Insert(CalOpe ope, int pos)
        {
            this.valList.Insert(pos, new CalVal(ope));
        }
        #endregion

        #region 将字符串表达式转换成运算因子(数字、常数及运算符)列表
        /// <summary>
        /// 将字符串表达式转换成运算因子列表
        /// </summary>
        /// <param name="str">字符串表达式</param>
        private void ToList(List<CalVal> valList, string str)
        {
            do
            {
                //字符串中首个运算符的位置
                int opePos = GetOpePos(str);

                CalNum num = null;
                CalOpe ope = null;

                if (opePos > 0)//找到运算符且它前面有数字
                {
                    num = GetNum(ref str, opePos);
                }
                else if (opePos == 0)//找到运算符且它在字符串最前面
                {
                    ope = GetOpe(ref str);
                }
                else//没有找到运算符
                {
                    num = GetNum(ref str);
                }

                if (num != null)
                {
                    valList.Add(new CalVal(num));
                }

                if (ope != null)
                {
                    valList.Add(new CalVal(ope));
                }
            } while (str != "");
        }
        #endregion

        #region 获取字符串中首个运算符或常数的位置
        /// <summary>
        /// 获取字符串中首个运算符或常数的位置
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>值若为-1表示未找到</returns>
        private int GetOpePos(string str)
        {
            CalOpeList opeList = new CalOpeList();

            int pos = -1;
            for (int i = 0; i < opeList.count; i++)
            {
                int k = -1;

                int opeIndex = str.IndexOf(opeList.opeList[i].tag);

                //科学计数法的情况：如果'+'或'-'的前面是'E'
                if (opeList.opeList[i].opeType == OpeType.PlusOrMinus
                    && opeIndex - 1 >= 0
                    && str.Substring(opeIndex - 1, 1) == "E")
                {
                    //从'+'或'-'的后面重新查找
                    k = str.Substring(opeIndex + 1).IndexOf(opeList.opeList[i].tag);

                    if (k != -1)//如果找到
                    {
                        //计算该运算符的位置
                        k += opeIndex + 1;
                    }
                }
                else
                {
                    k = opeIndex;
                }

                if (pos == -1)
                {
                    pos = k;
                }
                else if (k >= 0 && k < pos)
                {
                    pos = k;
                }
            }

            return pos;//值若为-1表示未找到
        }
        #endregion

        #region 获得运算符
        /// <summary>
        /// 获得运算符
        /// </summary>
        private CalOpe GetOpe(ref string str)
        {
            CalOpeList opeList = new CalOpeList();
            CalOpe ope = null;

            for (int i = 0; i < opeList.count; i++)
            {
                if (str.IndexOf(opeList.opeList[i].tag) == 0)
                {
                    ope = opeList.opeList[i];
                    //更新str
                    str = str.Substring(opeList.opeList[i].tag.Length);
                    break;
                }
            }

            return ope;
        }
        #endregion

        #region 获取数字
        /// <summary>
        /// 获取数字
        /// </summary>
        private CalNum GetNum(ref string str, int opePos)
        {
            CalNum result = null;

            result = new CalNum(str.Substring(0, opePos));

            //更新str
            str = str.Substring(opePos);

            return result;
        }

        /// <summary>
        /// 获取数字
        /// </summary>
        private CalNum GetNum(ref string str)
        {
            CalNum result = null;

            result = new CalNum(str);

            //更新str
            str = "";

            return result;
        }
        #endregion

        #region 计算最终结果
        /// <summary>
        /// 计算最终结果
        /// </summary>
        /// <returns>最终计算结果</returns>
        public CalResult Calculate()
        {
            CalResult result = null;

            //优先级列表为空的情况
            if (levelList.Count == 0)
            {
                result = CalculateOneStep();
            }

            //处理所有优先级
            while (levelList.Count > 0)
            {
                result = CalculateOneStep();
            }

            return result;
        }
        #endregion

        #region 单步计算
        /// <summary>
        /// 单步计算
        /// </summary>
        /// <returns>单步计算结果</returns>
        public CalResult CalculateOneStep()
        {
            CalResult result = null;
            int currentPos = -1;

            try
            {
                #region 优先级列表为空的情况
                //优先级列表不存在的情况
                if (levelList.Count == 0)
                {
                    #region 完成计算后(优先级列表为空，无需计算)，删除逗号
                    //扫描运算因子列表，删除逗号
                    for (int i = 0; i < valList.Count; i++)
                    {
                        //找到逗号
                        if (valList[i].OpeType == OpeType.Separator)
                        {
                            this.valList.RemoveAt(i);
                            //调整i
                            i--;
                        }
                    }
                    #endregion

                    #region 完成计算后(优先级列表为空，无需计算)，删除括号
                    //扫描运算因子列表，删除括号
                    for (int i = 0; i < valList.Count; i++)
                    {
                        //找到括号
                        if (valList[i].OpeType == OpeType.leftParenthesis
                            || valList[i].OpeType == OpeType.rightParenthesis)
                        {
                            this.valList.RemoveAt(i);
                            //调整i
                            i--;
                        }
                    }
                    #endregion

                    if (valList.Count == 1)//一个计算结果
                    {
                        //该运算因子是数字
                        if (valList[0].type == ValType.Number)
                        {
                            double r;
                            //如果无法转换成double值
                            if (!double.TryParse(valList[0].num.stringValue, out r))
                            {
                                throw new Exception("请输入正确的表达式");
                            }
                        }

                        result = new CalResult();
                        result.numList.Add(valList[0].num);
                    }
                    else//多个计算结果
                    {
                        result = new CalResult();
                        for (int i = 0; i < valList.Count; i++)
                        {
                            //该运算因子是数字
                            if (valList[i].type == ValType.Number)
                            {
                                double r;
                                //如果无法转换成double值
                                if (!double.TryParse(valList[i].num.stringValue, out r))
                                {
                                    throw new Exception("请输入正确的表达式");
                                }
                            }

                            result.numList.Add(valList[i].num);
                        }
                    }

                    return result;
                }
                #endregion

                #region 核心算法，对于当前优先级，扫描运算因子列表并计算
                if (levelList.Count != 0)//优先级列表不为空
                {
                    #region 处理当前优先级
                    //对于当前优先级，逐个扫描运算因子，处理具有该优先级的运算
                    for (int i = 0; i < valList.Count; i++)
                    {
                        //找到具有该优先级的运算符
                        if (valList[i].type == ValType.Operator
                            && valList[i].level == levelList[0])//该运算因子的优先级等于当前优先级
                        {
                            bool isSign = false;//Sign为true表示当前运算因子是正负号
                            currentPos = valList[i].primalPos;//记录当前运算因子在列表中的原始位置

                            //参数列表
                            List<CalNum> numList = new List<CalNum>();
                            //临时计算结果
                            CalNum num;

                            switch (valList[i].OpeType)//i表示该运算因子的位置
                            {
                                #region case OpeType.PlusOrMinus:
                                case OpeType.PlusOrMinus:
                                    //若该运算符前面没有运算因子
                                    //或前一个运算因子是运算符，则按正负号计算
                                    if (i == 0 || (i - 1 >= 0 && valList[i - 1].type == ValType.Operator))
                                    {
                                        isSign = true;//是正负号
                                        //获取该运算符的参数
                                        if (i + 1 < valList.Count && valList[i + 1].type == ValType.Number)
                                        {
                                            //注意，下面第一句不可省略
                                            numList = new List<CalNum>();

                                            numList.Add(valList[i + 1].num);
                                        }

                                        //计算
                                        num = CalOpeList.Calculate(valList[i].ope, numList);
                                        //更新运算因子列表
                                        ReplaceVal(num, 2, i);

                                        //i无需调整
                                    }
                                    else//若前一个运算因子是操作数，则按加减号计算
                                    {
                                        //获取该运算符的参数
                                        if (valList[i - 1].type == ValType.Number
                                            && i + 1 < valList.Count && valList[i + 1].type == ValType.Number)
                                        {
                                            //注意，下面第一句不可省略
                                            numList = new List<CalNum>();

                                            numList.Add(valList[i - 1].num);
                                            numList.Add(valList[i + 1].num);
                                        }

                                        //计算
                                        num = CalOpeList.Calculate(valList[i].ope, numList);
                                        //更新运算因子列表
                                        ReplaceVal(num, 3, i - 1);
                                        //调整i
                                        i--;
                                    }
                                    break;
                                #endregion

                                #region case OpeType.bothSides:
                                case OpeType.bothSides:
                                    //获取该运算符的参数
                                    if (i >= 1 && valList[i - 1].type == ValType.Number
                                        && i + 1 < valList.Count && valList[i + 1].type == ValType.Number)
                                    {
                                        //注意，下面第一句不可省略
                                        numList = new List<CalNum>();

                                        numList.Add(valList[i - 1].num);
                                        numList.Add(valList[i + 1].num);
                                    }

                                    //计算
                                    num = CalOpeList.Calculate(valList[i].ope, numList);
                                    //更新运算因子列表
                                    ReplaceVal(num, 3, i - 1);
                                    //调整i
                                    i--;
                                    break;
                                #endregion

                                #region case OpeType.left:
                                case OpeType.left:
                                    //获取该运算符的参数
                                    if (i >= 1 && valList[i - 1].type == ValType.Number)
                                    {
                                        //注意，下面第一句不可省略
                                        numList = new List<CalNum>();

                                        numList.Add(valList[i - 1].num);
                                    }

                                    //计算
                                    num = CalOpeList.Calculate(valList[i].ope, numList);
                                    //更新运算因子列表
                                    ReplaceVal(num, 2, i - 1);
                                    //调整i
                                    i--;
                                    break;
                                #endregion

                                #region case OpeType.noParameter:
                                case OpeType.noParameter:
                                    //注意，下面第一句不可省略
                                    numList = new List<CalNum>();

                                    //计算
                                    num = CalOpeList.Calculate(valList[i].ope, numList);
                                    //更新运算因子列表
                                    ReplaceVal(num, 1, i);
                                    break;
                                #endregion

                                #region case OpeType.right:
                                case OpeType.right:
                                    #region 该运算符只有一个参数，且它的右边是常数
                                    if (i + 1 < valList.Count
                                        && valList[i].ParameterNumber == 1
                                        && valList[i + 1].type == ValType.Number)
                                    {
                                        //获取该运算符的参数
                                        //注意，下面第一句不可省略
                                        numList = new List<CalNum>();
                                        numList.Add(valList[i + 1].num);

                                        //计算
                                        num = CalOpeList.Calculate(valList[i].ope, numList);
                                        //更新运算因子列表
                                        ReplaceVal(num, 2, i);
                                    }
                                    #endregion

                                    #region 该运算符不是只有一个参数，或者它的右边不是常数
                                    else
                                    {
                                        //计算参数个数
                                        int count = 0;

                                        int k = i + 1;
                                        //从运算符后面开始检测
                                        //如果是左括号、分隔符或数字，则执行while中语句
                                        while (k < valList.Count
                                            && (valList[k].type == ValType.Number
                                            || valList[k].OpeType == OpeType.Separator
                                            || valList[k].OpeType == OpeType.leftParenthesis))
                                        {
                                            //如果是数字，参数个数加1
                                            if (valList[k].type == ValType.Number)
                                            {
                                                count++;
                                            }
                                            k++;
                                        }

                                        //注意，下面第一句不可省略
                                        numList = new List<CalNum>();

                                        //从该运算符后面，逐个扫描，获取该运算符的参数
                                        //j表示已读取的参数个数
                                        //m表示检测位置增量
                                        int m = 0;
                                        for (int j = 0; j < count; )
                                        {
                                            //如果找到数字，存为参数
                                            if (valList[i + j + m + 1].type == ValType.Number)
                                            {
                                                numList.Add(valList[i + j + m + 1].num);
                                                j++;
                                            }
                                            //如果是分隔符或左括号，检测位置增量加1，表示跳过该运算因子，继续检测
                                            else if (valList[i + j + m + 1].OpeType == OpeType.Separator
                                                || valList[i + j + m + 1].OpeType == OpeType.leftParenthesis)
                                            {
                                                m++;
                                            }
                                        }

                                        //计算
                                        num = CalOpeList.Calculate(valList[i].ope, numList);
                                        //更新运算因子列表，count+m+2中的2表示运算符本身和右括号
                                        ReplaceVal(num, count + m + 2, i);
                                    }
                                    #endregion

                                    break;

                                #endregion

                            }//end switch

                            if (!isSign)//如果不是正负号
                            {
                                break;//退出for循环
                            }
                        }
                    }
                    #endregion

                    #region 删除处理完的优先级
                    bool levelExists = false;//是否存在具有该优先级的运算符
                    //逐个扫描运算因子,判断是否仍存在具有该优先级的运算符
                    for (int i = 0; i < valList.Count; i++)
                    {
                        if (levelList[0] == valList[i].level)//存在
                        {
                            levelExists = true;
                        }
                    }
                    if (!levelExists)//该优先级已处理完则删除它
                    {
                        levelList.RemoveAt(0);
                    }
                    #endregion

                }
                #endregion

                #region 处理计算结果
                //处理计算结果
                if (levelList.Count == 0)//全部优先级已经处理完毕
                {
                    #region 完成计算后，删除逗号
                    //扫描运算因子列表，删除逗号
                    for (int i = 0; i < valList.Count; i++)
                    {
                        //找到逗号
                        if (valList[i].OpeType == OpeType.Separator)
                        {
                            this.valList.RemoveAt(i);
                            //调整i
                            i--;
                        }
                    }
                    #endregion

                    #region 完成计算后，删除括号
                    //扫描运算因子列表，删除括号
                    for (int i = 0; i < valList.Count; i++)
                    {
                        //找到括号
                        if (valList[i].OpeType == OpeType.leftParenthesis
                            || valList[i].OpeType == OpeType.rightParenthesis)
                        {
                            this.valList.RemoveAt(i);
                            //调整i
                            i--;
                        }
                    }
                    #endregion

                    if (valList.Count == 1)//一个计算结果
                    {
                        result = new CalResult();
                        result.numList.Add(valList[0].num);
                    }
                    else//多个计算结果
                    {
                        result = new CalResult();
                        for (int i = 0; i < valList.Count; i++)
                        {
                            result.numList.Add(valList[i].num);
                        }
                    }
                }
                else//全部优先级未处理完
                {
                    result = new CalResult();
                    result.numList.Add(new CalNum(BackToStrExp()));//剩下的运算因子列表转换成string后作为计算结果返回
                }
                #endregion

            }
            catch (Exception ex)
            {
                if (currentPos != -1)
                {
                    MakeException(currentPos, ex);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }
        #endregion

        #region 替换运算因子
        /// <summary>
        /// 替换运算因子
        /// </summary>
        /// <param name="num">用于替换的操作数</param>
        /// <param name="count">要替换的运算因子的个数</param>
        /// <param name="pos">替换位置</param>
        private void ReplaceVal(CalNum num, int count, int pos)
        {
            valList[pos] = new CalVal(num);
            valList.RemoveRange(pos + 1, count - 1);

            //左右括号同时存在，处理左右括号
            if (pos >= 1
                && valList[pos - 1].OpeType == OpeType.leftParenthesis
                && pos + 1 < valList.Count
                && valList[pos + 1].OpeType == OpeType.rightParenthesis)
            {
                //如sin(9-7),average(9-7)
                if (!(pos >= 2
                    && valList[pos - 2].OpeType == OpeType.right))
                {
                    //删除左右括号
                    valList[pos - 1] = valList[pos];
                    valList.RemoveRange(pos, 2);
                }
            }
        }
        #endregion

        #region 将优先级插入到优先级列表中
        /// <summary>
        /// 将优先级插入到优先级列表中
        /// </summary>
        /// <param name="levelList">优先级列表</param>
        /// <param name="level">要插入的优先级</param>
        private void InsertIntoLevelList(ref List<int> levelList, int level)
        {
            //列表为空的情况
            if (levelList.Count == 0)
            {
                levelList.Add(level);
                return;
            }

            //该优先级是否已存在
            for (int i = 0; i < levelList.Count; i++)
            {
                //该优先级已存在
                if (level == levelList[i])
                {
                    return;
                }
            }

            int k = 0;
            for (; k < levelList.Count; k++)
            {
                if (level > levelList[k])
                {
                    levelList.Insert(k, level);
                    break;
                }
            }
            if (k >= levelList.Count)
            {
                levelList.Add(level);
            }
        }
        #endregion

        #region 设置运算因子在运算因子列表的原始位置(规范化后)
        /// <summary>
        /// 设置运算因子在运算因子列表的原始位置(规范化后)
        /// </summary>
        private void SetValPrimalPos()
        {
            for (int i = 0; i < valList.Count; i++)
            {
                valList[i].primalPos = i;
            }
        }
        #endregion

        #region 生成优先级列表
        /// <summary>
        /// 生成优先级列表
        /// </summary>
        private void MakeLevelList()
        {
            #region 生成优先级列表，优先级按从高到低存储
            //逐个扫描运算因子，提取优先级列表，并按优先级从高到低存储
            for (int i = 0; i < valList.Count; i++)
            {
                //如果是运算符
                if (valList[i].type == ValType.Operator)
                {
                    //如果是括号或分隔符
                    if (valList[i].OpeType == OpeType.leftParenthesis
                        || valList[i].OpeType == OpeType.rightParenthesis
                        || valList[i].OpeType == OpeType.Separator)
                    {
                        continue;
                    }
                    InsertIntoLevelList(ref levelList, valList[i].level);
                }
            }
            #endregion
        }
        #endregion

    }
}
