def find_bridges(graph):
    time = [0] 
    def bridge_util(u, visited, disc, low, parent, bridges):
        visited[u] = True
        time[0] += 1
        disc[u] = low[u] = time[0]

        for v in graph[u]:
            if not visited[v]:  
                parent[v] = u
                bridge_util(v, visited, disc, low, parent, bridges)


                low[u] = min(low[u], low[v])
                if low[v] > disc[u]:
                    bridges.append((u, v))

            elif v != parent[u]:  
                low[u] = min(low[u], disc[v])

    V = len(graph)
    visited = [False] * V
    disc = [float('inf')] * V
    low = [float('inf')] * V
    parent = [-1] * V
    bridges = []

    for i in range(V):
        if not visited[i]:
            bridge_util(i, visited, disc, low, parent, bridges)

    return bridges


if __name__ == "__main__":

    vertices = int(input("Введите количество вершин: "))
    graph = [[] for _ in range(vertices)]

    print("Введите ребра в формате 'u v' (для выхода введите 'exit'):")
    while True:
        line = input()
        if line.strip().lower() == 'exit':
            break
        u, v = map(int, line.split())
        graph[u].append(v)
        graph[v].append(u)

    bridges = find_bridges(graph)
    print("Мосты в графе:")
    for u, v in bridges:
        print(f"{u} -- {v}")
