class UnionFind:
    def __init__(self, n):
        self.parent = list(range(n))
        self.rank = [0] * n

    def find(self, u):
        if self.parent[u] != u:
            self.parent[u] = self.find(self.parent[u]) 
        return self.parent[u]

    def union(self, u, v):
        root_u = self.find(u)
        root_v = self.find(v)

        if root_u != root_v:
            if self.rank[root_u] > self.rank[root_v]:
                self.parent[root_v] = root_u
            elif self.rank[root_u] < self.rank[root_v]:
                self.parent[root_u] = root_v
            else:
                self.parent[root_v] = root_u
                self.rank[root_u] += 1


def kruskal_algo(edges, num_vertices):
    uf = UnionFind(num_vertices)
    mst = []
    total_weight = 0

    for edge in edges:
        u, v, w = edge
        if uf.find(u) != uf.find(v):
            uf.union(u, v)
            mst.append(edge)
            total_weight += w

    return mst, total_weight


def main():
    edges = [
        ('A', 'B', 4),
        ('A', 'H', 8),
        ('B', 'C', 8),
        ('B', 'H', 11),
        ('C', 'D', 7),
        ('C', 'F', 4),
        ('C', 'I', 2),
        ('D', 'E', 9),
        ('D', 'F', 14),
        ('E', 'F', 10),
        ('F', 'G', 2),
        ('G', 'H', 1),
        ('G', 'I', 6),
        ('H', 'I', 7)
    ]

    vertices = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I']
    num_vertices = len(vertices)

    vertex_map = {v: i for i, v in enumerate(vertices)}
    mapped_edges = [(vertex_map[u], vertex_map[v], w) for u, v, w in edges]

    sorted_edges = sorted(mapped_edges, key=lambda e: e[2])

    mst, total_weight = kruskal_algo(sorted_edges, num_vertices)

    result_edges = [(vertices[e[0]], vertices[e[1]]) for e in mst]

    print("Минимальное остовное дерево:", result_edges)
    print("Общая длина MST:", total_weight)


if __name__ == "__main__":
    main()
