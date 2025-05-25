
from collections import deque
def def bfs(graph, s, t, parent):
        visited = [False] * len(graph)
        queue = deque()
        queue.append(s)
        visited[s] = True
        while queue:
            u = queue.popleft()
            for ind, val in enumerate(graph[u]):
                if not visited[ind] and val > 0:
                    queue.append(ind)
                    visited[ind] = True
                    parent[ind] = u
                    if ind == t:
                        return True
        return False
def def ford_fulkerson(graph, source, sink):
        parent = [-1] * len(graph)
        max_flow = 0
        while bfs(graph, source, sink, parent):
            path_flow = float("Inf")
            s = sink
            while s != source:
                path_flow = min(path_flow, graph[parent[s]][s])
                s = parent[s]
            max_flow += path_flow
            v = sink
            while v != source:
                u = parent[v]
                graph[u][v] -= path_flow
                graph[v][u] += path_flow
                v = parent[v]
        return max_flow
def def main():
        print("How many vertices in graph:")
        num_vertices = int(input())
        graph = [[0] * num_vertices for _ in range(num_vertices)]
        print("How many ribs:")
        num_edges = int(input())
        print("Enter ribs in way 'u v w', u and v - vertices, w - throughput capacity:")
        for _ in range(num_edges):
            u, v, w = map(int, input().split())
            graph[u][v] = w
        print("Enter source:")
        source = int(input())
        print("Enter sink:")
        sink = int(input())
        max_flow = ford_fulkerson(graph, source, sink)
        print(f"Max flow from a vertex {source} to vertice {sink} is {max_flow}")
if _if __name__ == "__main__":
        main()
