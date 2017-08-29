using UnityEngine;
using System;

public class BreadCrumb : IComparable<BreadCrumb>
{	
	public Point position;
	public BreadCrumb prev;
	public BreadCrumb next;
	public int cost = Int32.MaxValue;
	public bool onClosedList = false;
	public bool onOpenList = false;

    public BreadCrumb(Point position)
    {
        this.position = position;
    }
		
	//Overrides default Equals so we check on position instead of object memory location
	public override bool Equals(object obj)
	{
		return (obj is BreadCrumb) && ((BreadCrumb)obj).position.X == this.position.X && ((BreadCrumb)obj).position.Y == this.position.Y;
	}
	
	//Faster Equals for if we know something is a BreadCrumb
	public bool Equals(BreadCrumb breadcrumb)
	{
	    return breadcrumb.position.X == this.position.X && breadcrumb.position.Y == this.position.Y;
	}
	
	public override int GetHashCode()
	{
	    return position.GetHashCode();
	}
	
	#region IComparable<> interface
	public int CompareTo(BreadCrumb other)
	{
	    return cost.CompareTo(other.cost);
	}
	#endregion
}

