Imports System.Data.Entity

Public Class Repository(Of TEntity As Class)
    Implements IRepository(Of TEntity)

    Property _context As DbContext

    Public Sub New(context As DbContext)
        _context = context
    End Sub

    Public ReadOnly Property Table As DbSet(Of TEntity) Implements IRepository(Of TEntity).Table
        Get
            Return _context.Set(Of TEntity)()
        End Get
    End Property

    Public Sub Add(ByRef entity As TEntity) Implements IRepository(Of TEntity).Add
        Table.Add(entity)
    End Sub

    Public Sub Update(ByRef entity As TEntity) Implements IRepository(Of TEntity).Update
        Table.Attach(entity)
    End Sub

    Public Sub Delete(ByRef entity As TEntity) Implements IRepository(Of TEntity).Delete
        Table.Remove(entity)
    End Sub
End Class