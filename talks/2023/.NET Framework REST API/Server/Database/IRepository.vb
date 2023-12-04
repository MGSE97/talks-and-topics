Imports System.Data.Entity

Public Interface IRepository(Of TEntity As Class)
    ReadOnly Property Table() As DbSet(Of TEntity)
    Sub Add(ByRef entity As TEntity)
    Sub Update(ByRef entity As TEntity)
    Sub Delete(ByRef entity As TEntity)
End Interface
