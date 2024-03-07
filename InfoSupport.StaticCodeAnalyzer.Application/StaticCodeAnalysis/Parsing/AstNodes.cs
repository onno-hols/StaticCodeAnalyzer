﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.StaticCodeAnalyzer.Application.StaticCodeAnalysis.Parsing;

public abstract class AstNode
{
    public List<Token> Tokens { get; set; } = [];
}

public class RootNode : AstNode
{
    public List<GlobalStatementNode> GlobalStatements { get; set; } = [];
}

public class StatementNode : AstNode
{

}

public class GlobalStatementNode : AstNode
{
    public required StatementNode Statement { get; set; }
}

public class ExpressionStatementNode : StatementNode
{
    public required ExpressionNode Expression { get; set; }
}

public class ExpressionNode : AstNode
{

}

public class LiteralExpressionNode : ExpressionNode
{

}

[DebuggerDisplay("{ToString()}")]
public class NumericLiteralNode : LiteralExpressionNode
{
    public object? Value { get; set; }

    public override string ToString() => $"{Value}";
}

[DebuggerDisplay("{ToString()}")]
public class BooleanLiteralNode : LiteralExpressionNode
{
    public bool Value { get; set; }

    public override string ToString() => $"{Value}";
}

[DebuggerDisplay("{ToString()}")]
public class StringLiteralNode : LiteralExpressionNode
{
    public required string Value { get; set; }

    public override string ToString() => $"{Value}";
}

[DebuggerDisplay("{ToString()}")]
public class ParenthesizedExpression : ExpressionNode
{
    public ExpressionNode Expression { get; set; }

    public ParenthesizedExpression(ExpressionNode expr) => Expression = expr;

    public override string ToString() => $"({Expression})";
}



public enum UnaryOperator
{
    Negation,
    LogicalNot,
    Increment,
    Decrement
}

[DebuggerDisplay("{ToString()}")]
public class UnaryExpressionNode : ExpressionNode
{
    public ExpressionNode Expression { get; set; }
    public bool IsPrefix { get; set; } = true;

    public virtual UnaryOperator Operator { get; }

    public UnaryExpressionNode(ExpressionNode expr, bool isPrefix = true)
    {
        Expression = expr;
        IsPrefix = isPrefix;
    }

    private string OperatorForDbg => Operator switch
    {
        UnaryOperator.Negation => "-",
        UnaryOperator.LogicalNot => "!",
        UnaryOperator.Increment => "++",
        UnaryOperator.Decrement => "--",
        _ => throw new NotImplementedException()
    };

    public override string ToString() => $"{(IsPrefix ? OperatorForDbg : "")}{Expression}{(!IsPrefix ? OperatorForDbg : "")}";
}

public class UnaryNegationNode(ExpressionNode expr, bool isPrefix = true) : UnaryExpressionNode(expr, isPrefix)
{
    public override UnaryOperator Operator => UnaryOperator.Negation;
}

public enum BinaryOperator
{
    // Arithmetic
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulus,

    // Boolean
    Equals,
    NotEquals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,

    // ...
}

[DebuggerDisplay("{ToString()}")]
public class BinaryExpressionNode : ExpressionNode
{
    public ExpressionNode LHS { get; set; }
    public ExpressionNode RHS { get; set; }

    public virtual BinaryOperator Operator { get; }

    public BinaryExpressionNode(ExpressionNode lhs, ExpressionNode rhs)
    {
        LHS = lhs;
        RHS = rhs;
    }

    private string OperatorForDbg => Operator switch
    {
        BinaryOperator.Add => "+",
        BinaryOperator.Subtract => "-",
        BinaryOperator.Multiply => "*",
        BinaryOperator.Divide => "/",
        BinaryOperator.Modulus => "%",
        _ => throw new NotImplementedException()
    };

    public override string ToString() => $"{LHS} {OperatorForDbg} {RHS}";
}

public class AddExpressionNode(ExpressionNode lhs, ExpressionNode rhs) : BinaryExpressionNode(lhs, rhs)
{
    public override BinaryOperator Operator { get => BinaryOperator.Add; }
}

public class MultiplyExpressionNode(ExpressionNode lhs, ExpressionNode rhs) : BinaryExpressionNode(lhs, rhs)
{
    public override BinaryOperator Operator { get => BinaryOperator.Multiply; }
}

public class SubtractExpressionNode(ExpressionNode lhs, ExpressionNode rhs) : BinaryExpressionNode(lhs, rhs)
{
    public override BinaryOperator Operator { get => BinaryOperator.Subtract; }
}

public class DivideExpressionNode(ExpressionNode lhs, ExpressionNode rhs) : BinaryExpressionNode(lhs, rhs)
{
    public override BinaryOperator Operator { get => BinaryOperator.Divide; }
}

public class ModulusExpressionNode(ExpressionNode lhs, ExpressionNode rhs) : BinaryExpressionNode(lhs, rhs)
{
    public override BinaryOperator Operator { get => BinaryOperator.Modulus; }
}

public class EqualsExpressionNode(ExpressionNode lhs, ExpressionNode rhs) : BinaryExpressionNode(lhs, rhs)
{
    public override BinaryOperator Operator { get => BinaryOperator.Equals; }
}

public class NotEqualsExpressionNode(ExpressionNode lhs, ExpressionNode rhs) : BinaryExpressionNode(lhs, rhs)
{
    public override BinaryOperator Operator { get => BinaryOperator.NotEquals; }
}

public class TernaryExpressionNode : ExpressionNode
{

}

// @FIXME is an identifier an expression and not just a part of an expression?
[DebuggerDisplay("{Identifier,nq}")]
public class IdentifierExpression : ExpressionNode
{
    public required string Identifier { get; set; }

    public override string ToString() => Identifier;
}